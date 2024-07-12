using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyShop.Application.Abstractions;
using MyShop.Application.Commands.Auth;
using MyShop.Application.Dtos.Auth;
using MyShop.Application.Mappings;
using MyShop.Application.Utils;
using MyShop.Core.Abstractions;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.Shared;
using MyShop.Infrastructure.InfrastructureServices;
using MyShop.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyShop.Infrastructure.Security;
internal sealed class Authenticator(
    IUnitOfWork unitOfWork,
    IOptions<AuthOptions> options,
    IPasswordManager passwordManager,
    TimeProvider timeProvider,
    IIdentifyPlatformService identifyPlatformService
    ) : IAuthenticator
{
    private readonly string _issuer = options.Value.Issuer;
    private readonly TimeSpan _expiryAccessToken = options.Value.ExpiryAccessToken ?? TimeSpan.FromHours(1);
    private readonly TimeSpan _expiryRefreshToken = options.Value.ExpiryRefreshToken ?? TimeSpan.FromHours(2);
    private readonly string _audience = options.Value.Audience;
    private readonly SigningCredentials _signingCredentials = new(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SigningKey)),
        SecurityAlgorithms.HmacSha256
        );
    private readonly JwtSecurityTokenHandler _jwtSecurityToken = new();

    public async Task<AuthDto> AuthenticateAsync(Email email, string password, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nameof(email));
        ArgumentException.ThrowIfNullOrWhiteSpace(nameof(password));

        var user = await unitOfWork.RegisteredUserRepository.GetFirstByPredicateAsync(
            predicate: p => p.Email.Equals(email),
            includeExpression: i => i.UserTokens,
            withTracking: true,
            cancellationToken: cancellationToken
            );

        if (user is null || !passwordManager.Verify(password, user.SecuredPassword))
        {
            throw new BadRequestException("Invalid Email or Password.");
        }

        var now = timeProvider
            .GetUtcNow();

        var platformInfo = identifyPlatformService.GetRequestPlatformInfo();
        var (newRefreshToken, newSecuredRefreshToken) = GenerateSecuredRefreshToken();

        var userToken = user.AddUserToken(newSecuredRefreshToken, platformInfo, now.Add(_expiryRefreshToken));

        var (token, expiryAccessTokenDate) = GenerateToken(user, userToken.Id, now);

        user.RemoveExpiredTokens(now);

        await unitOfWork.UserTokenRepository.AddAsync(userToken, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return userToken.ToAuthDto(token, newRefreshToken, expiryAccessTokenDate);
    }

    public async Task SignUpCustomerAsync(
        SignUpCutomerAuth command,
        CancellationToken cancellationToken = default
        )
    {
        if (await unitOfWork.RegisteredUserRepository.AnyAsync(x => Convert.ToString(x.Email).ToLower().Equals(command.Email.ToLower()), cancellationToken))
        {
            throw new BadRequestException($"Account with {nameof(RegisteredUser.Email)} equals '{command.Email}' exist.");
        }

        var customer = new Customer(
            command.FirstName,
            command.LastName,
            command.Email,
            command.Gender,
            passwordManager.SecurePassword(command.Password),
            command.DateOfBirth,
            command.PhoneNumber
        );

        await unitOfWork.UserRepository.AddAsync(customer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<AuthDto> SignUpGuestAsync(CancellationToken cancellationToken = default)
    {
        var now = timeProvider
            .GetUtcNow();

        var platformInfo = identifyPlatformService.GetRequestPlatformInfo();

        var (newRefreshToken, newSecuredRefreshToken) = GenerateSecuredRefreshToken();

        var guest = new Guest();

        var guestToken = guest.AddUserToken(
            newSecuredRefreshToken,
            platformInfo,
            now.Add(_expiryRefreshToken)
            );

        var (accessToken, expiryAccessTokenDate) = GenerateToken(guest, guestToken.Id, now);


        await unitOfWork.UserRepository.AddAsync(guest, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return guestToken.ToAuthDto(accessToken, newRefreshToken, expiryAccessTokenDate);
    }

    public async Task<AuthDto> RefreshTokenAsync(RefreshAccessTokenAuth refreshAccessToken, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(refreshAccessToken);
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshAccessToken.RefreshToken);

        var now = timeProvider
            .GetUtcNow();

        var platformInfo = identifyPlatformService.GetRequestPlatformInfo();

        var userToken = await unitOfWork.UserTokenRepository.GetByIdAsync(
            id: refreshAccessToken.UserTokenId,
            includeExpression: i => i.User,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new BadRequestException();

        if (!passwordManager.Verify(refreshAccessToken.RefreshToken, userToken.RefreshToken))
        {
            throw new BadRequestException("Invalid refresh token.");
        }

        var (accessToken, expiryAccessTokenDate) = GenerateToken(userToken.User, userToken.Id, now);

        var (newRefreshToken, newSecuredRefreshToken) = GenerateSecuredRefreshToken();

        userToken = userToken.Update(
           newSecuredRefreshToken,
           now.Add(_expiryRefreshToken),
           platformInfo,
           now
           );

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return userToken.ToAuthDto(accessToken, newRefreshToken, expiryAccessTokenDate);
    }

    private (string Token, DateTimeOffset ExpiryAccessTokenDate) GenerateToken(User user, Guid userTokenId, DateTimeOffset now)
    {
        ArgumentNullException.ThrowIfNull(nameof(user));

        if (now == default)
        {
            throw new ArgumentException($"Parameter {nameof(now)} cannot be default.", nameof(now));
        }

        var claims = user switch
        {
            Customer cutomer => GetCustomerClaims(cutomer, userTokenId),
            Employee employee => GetEmployeeClaims(employee, userTokenId),
            Guest guest => GetGuestClaims(guest, userTokenId),
            _ => throw new ServerException($"Unexpected exception with parameter {nameof(user)} instance.)")
        };

        var expiryAccessTokenDate = now.Add(_expiryAccessToken);
        var jwt = new JwtSecurityToken(_issuer, _audience, claims, now.UtcDateTime, expiryAccessTokenDate.UtcDateTime, _signingCredentials);


        return (_jwtSecurityToken.WriteToken(jwt), expiryAccessTokenDate);
    }

    private (string RefreshToken, string SecuredRefreshToken) GenerateSecuredRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        var refreshToken = Convert.ToBase64String(randomNumber);
        var securedRefreshToken = passwordManager.SecureRefreshToken(refreshToken);

        return (refreshToken, securedRefreshToken);
    }

    private static Claim[] GetCustomerClaims(Customer customer, Guid userTokenId)
        =>
        [
            new(ClaimTypes.NameIdentifier, customer.Id.ToString()),
            new(ClaimTypes.Email, customer.Email),
            new(ClaimTypes.Role, customer.Role),
            new(ClaimTypes.Gender, customer.Gender),
            new(ClaimTypes.Name, customer.FirstName),
            new(ClaimTypes.DateOfBirth, customer.DateOfBirth.Value.ToShortDateString()),
            new(CustomClaimTypes.UserTokenId, userTokenId.ToString()),
        ];

    private static Claim[] GetEmployeeClaims(Employee employee, Guid userTokenId)
        =>
        [
            new(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new(ClaimTypes.Email, employee.Email),
            new(ClaimTypes.Role, employee.Role),
            new(CustomClaimTypes.EmployeeRole,  employee.EmployeeRole),
            new(ClaimTypes.Gender, employee.Gender),
            new(ClaimTypes.Name, employee.FirstName),
            new(ClaimTypes.DateOfBirth, employee.DateOfBirth.Value.ToShortDateString()),
            new(CustomClaimTypes.UserTokenId, userTokenId.ToString()),
        ];

    private static Claim[] GetGuestClaims(Guest guest, Guid userTokenId)
        =>
        [
            new(ClaimTypes.NameIdentifier, guest.Id.ToString()),
            new(ClaimTypes.Role, guest.Role),
            new(CustomClaimTypes.UserTokenId, userTokenId.ToString()),
        ];
}
