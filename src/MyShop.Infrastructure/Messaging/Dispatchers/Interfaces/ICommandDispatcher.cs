using MyShop.Application.Commands;

namespace MyShop.Infrastructure.Messaging.Dispatchers.Interfaces;
internal interface ICommandDispatcher
{
    Task HandleAsync(ICommand @event, CancellationToken cancellationToken = default);
}
