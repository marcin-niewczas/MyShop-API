using MyShop.Application.Abstractions;
using MyShop.Application.Commands.Auth;

namespace MyShop.Application.CommandHandlers.Auth;
internal sealed class SignUpCustomerAuthCommandHandler(
    IAuthenticator authenticator
    ) : ICommandHandler<SignUpCutomerAuth>
{
    public Task HandleAsync(SignUpCutomerAuth command, CancellationToken cancellationToken = default)
        => authenticator.SignUpCustomerAsync(command, cancellationToken);
}
