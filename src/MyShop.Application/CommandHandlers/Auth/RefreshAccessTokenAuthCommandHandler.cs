using MyShop.Application.Abstractions;
using MyShop.Application.Commands.Auth;
using MyShop.Application.Dtos.Auth;
using MyShop.Application.Responses;

namespace MyShop.Application.CommandHandlers.Auth;
internal sealed class RefreshAccessTokenAuthCommandHandler(
    IAuthenticator authenticator
    ) : ICommandHandler<RefreshAccessTokenAuth, ApiResponse<AuthDto>>
{
    public async Task<ApiResponse<AuthDto>> HandleAsync(
        RefreshAccessTokenAuth command, 
        CancellationToken cancellationToken = default
        ) => new(await authenticator.RefreshTokenAsync(command, cancellationToken));
}
