using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.Abstractions;
using MyShop.Infrastructure.Messaging.Dispatchers.Interfaces;

namespace MyShop.Infrastructure.Messaging.Dispatchers;
internal sealed class EventDispatcher(
    IServiceProvider serviceProvider
    ) : IEventDispatcher
{
    public async Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());

        var handler = scope.ServiceProvider.GetRequiredService(handlerType);

        await (Task)handlerType
               .GetMethod(nameof(IEventHandler<IEvent>.HandleAsync))
               ?.Invoke(handler, [@event, cancellationToken])!;
    }
}
