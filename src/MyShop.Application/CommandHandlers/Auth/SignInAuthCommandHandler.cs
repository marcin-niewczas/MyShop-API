using MyShop.Application.Abstractions;
using MyShop.Application.Commands.Auth;
using MyShop.Application.Dtos.Auth;
using MyShop.Application.Responses;

namespace MyShop.Application.CommandHandlers.Auth;
internal sealed class SignInAuthCommandHandler(
    IAuthenticator authenticator
    ) : ICommandHandler<SignInAuth, ApiResponse<AuthDto>>
{
    public async Task<ApiResponse<AuthDto>> HandleAsync(
        SignInAuth command,
        CancellationToken cancellationToken = default
        ) => new(await authenticator.AuthenticateAsync(command.Email, command.Password, cancellationToken));
}
