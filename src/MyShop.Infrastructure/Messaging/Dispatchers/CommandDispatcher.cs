using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands;
using MyShop.Infrastructure.Messaging.Dispatchers.Interfaces;

namespace MyShop.Infrastructure.Messaging.Dispatchers;
internal sealed class CommandDispatcher(
    IServiceProvider serviceProvider
    ) : ICommandDispatcher
{
    public async Task HandleAsync(ICommand @event, CancellationToken cancellationToken = default)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var handlerType = typeof(ICommandHandler<>).MakeGenericType(@event.GetType());

        var handler = scope.ServiceProvider.GetRequiredService(handlerType);

        await (Task)handlerType
               .GetMethod(nameof(ICommandHandler<ICommand>.HandleAsync))
               ?.Invoke(handler, [@event, cancellationToken])!;
    }
}
