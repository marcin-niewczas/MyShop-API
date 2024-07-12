using MyShop.Application.Commands.Auth;
using MyShop.Application.Dtos.Auth;
using MyShop.Core.ValueObjects.Shared;

namespace MyShop.Application.Abstractions;
public interface IAuthenticator
{
    Task<AuthDto> AuthenticateAsync(Email email, string password, CancellationToken cancellationToken = default);
    Task<AuthDto> RefreshTokenAsync(RefreshAccessTokenAuth refreshAccessToken, CancellationToken cancellationToken = default);
    Task SignUpCustomerAsync(
        SignUpCutomerAuth command,
        CancellationToken cancellationToken = default
        );
    Task<AuthDto> SignUpGuestAsync(CancellationToken cancellationToken = default);
}
