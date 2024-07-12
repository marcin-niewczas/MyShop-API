using MyShop.Application.Abstractions;

namespace MyShop.Infrastructure.Messaging.Dispatchers.Interfaces;
internal interface IAsyncMessageDispatcher
{
    Task PublishAsync<TMessage>(TMessage message) where TMessage : class, IMessage;
}
