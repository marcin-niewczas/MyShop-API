using MyShop.Application.Abstractions;
using MyShop.Application.Commands.Auth;
using MyShop.Application.Dtos.Auth;
using MyShop.Application.Responses;

namespace MyShop.Application.CommandHandlers.Auth;
internal sealed class SignUpGuestAuthCommandHandler(
    IAuthenticator authenticator
    ) : ICommandHandler<SignUpGuestAuth, ApiResponse<AuthDto>>
{
    public async Task<ApiResponse<AuthDto>> HandleAsync(SignUpGuestAuth command, CancellationToken cancellationToken = default)
        => new(await authenticator.SignUpGuestAsync(cancellationToken));
}
