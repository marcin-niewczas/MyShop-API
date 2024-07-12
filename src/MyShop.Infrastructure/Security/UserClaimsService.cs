using Microsoft.AspNetCore.Http;
using MyShop.Application.Abstractions;
using MyShop.Application.Utils;
using MyShop.Core.Exceptions;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.Users;
using System.Security.Claims;

namespace MyShop.Infrastructure.Security;
internal sealed class UserClaimsService(
    IHttpContextAccessor httpContextAccessor
    ) : IUserClaimsService
{
    private readonly ClaimsPrincipal _user = httpContextAccessor.HttpContext?.User
        ?? throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext.User));

    public UserClaimsData GetUserClaimsData()
    {
        var userRole = _user.FindFirstValue(ClaimTypes.Role)
                ?? throw new ServerException($"Cannot parse {nameof(UserRole)} from JwtClaims.");

        return userRole switch
        {
            UserRole.Employee => GetEmployeeUserClaimsData(userRole),
            UserRole.Customer => GetCustomerUserClaimsData(userRole),
            UserRole.Guest => GetGuestUserClaimsData(userRole),
            _ => throw new ServerException($"Unexpected {nameof(UserRole)} '{userRole}'.")
        };
    }

    private UserClaimsData GetGuestUserClaimsData(string userRole)
        => new()
        {
            UserId = Guid.TryParse(_user.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId)
                ? userId
                : throw new ServerException($"Cannot parse {nameof(UserAddress.RegisteredUserId)} from JwtClaims."),
            UserTokenId = Guid.TryParse(_user.FindFirstValue(CustomClaimTypes.UserTokenId), out Guid userTokenId)
                ? userTokenId
                : throw new ServerException($"Cannot parse {nameof(UserToken)}{nameof(IEntity.Id)} from JwtClaims."),
            UserRole = userRole
        };

    private CustomerClaimsData GetCustomerUserClaimsData(string userRole)
        => new()
        {
            UserId = Guid.TryParse(_user.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId)
                ? userId
                : throw new ServerException($"Cannot parse {nameof(UserAddress.RegisteredUserId)} from JwtClaims."),
            Email = _user.FindFirstValue(ClaimTypes.Email)
                ?? throw new ServerException($"Cannot parse {nameof(RegisteredUser.Email)} from JwtClaims."),
            FirstName = _user.FindFirstValue(ClaimTypes.Name)
                ?? throw new ServerException($"Cannot parse {nameof(RegisteredUser.FirstName)} from JwtClaims."),
            Gender = _user.FindFirstValue(ClaimTypes.Gender)
                ?? throw new ServerException($"Cannot parse {nameof(RegisteredUser.Gender)} from JwtClaims."),
            DateOfBirth = DateOnly.TryParse(_user.FindFirstValue(ClaimTypes.DateOfBirth), out var dateOnly)
                ? dateOnly
                : throw new ServerException($"Cannot parse {nameof(RegisteredUser.DateOfBirth)} from JwtClaims."),
            UserTokenId = Guid.TryParse(_user.FindFirstValue(CustomClaimTypes.UserTokenId), out Guid userTokenId)
                ? userTokenId
                : throw new ServerException($"Cannot parse {nameof(UserToken)}{nameof(IEntity.Id)} from JwtClaims."),
            UserRole = userRole
        };

    private EmployeeClaimsData GetEmployeeUserClaimsData(string userRole)
        => new()
        {
            UserId = Guid.TryParse(_user.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId)
                ? userId
                : throw new ServerException($"Cannot parse {nameof(UserAddress.RegisteredUserId)} from JwtClaims."),
            Email = _user.FindFirstValue(ClaimTypes.Email)
                ?? throw new ServerException($"Cannot parse {nameof(RegisteredUser.Email)} from JwtClaims."),
            FirstName = _user.FindFirstValue(ClaimTypes.Name)
                ?? throw new ServerException($"Cannot parse {nameof(RegisteredUser.FirstName)} from JwtClaims."),
            Gender = _user.FindFirstValue(ClaimTypes.Gender)
                ?? throw new ServerException($"Cannot parse {nameof(RegisteredUser.Gender)} from JwtClaims."),
            DateOfBirth = DateOnly.TryParse(_user.FindFirstValue(ClaimTypes.DateOfBirth), out var dateOnly)
                ? dateOnly
                : throw new ServerException($"Cannot parse {nameof(RegisteredUser.DateOfBirth)} from JwtClaims."),
            UserTokenId = Guid.TryParse(_user.FindFirstValue(CustomClaimTypes.UserTokenId), out Guid userTokenId)
                ? userTokenId
                : throw new ServerException($"Cannot parse {nameof(UserToken)}{nameof(IEntity.Id)} from JwtClaims."),
            UserRole = userRole,
            EmployeeRole = _user.FindFirstValue(CustomClaimTypes.EmployeeRole)
                ?? throw new ServerException($"Cannot parse {nameof(EmployeeRole)} from JwtClaims.")
        };
}
