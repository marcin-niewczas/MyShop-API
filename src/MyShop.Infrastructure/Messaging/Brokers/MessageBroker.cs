using MyShop.Application.Abstractions;
using MyShop.Infrastructure.Messaging.Dispatchers.Interfaces;

namespace MyShop.Infrastructure.Messaging.Brokers;
internal sealed class MessageBroker(
    IAsyncMessageDispatcher asyncMessageDispatcher
    ) : IMessageBroker
{
    public async Task PublishAsync(params IMessage[] messages)
    {
        if (messages is null)
        {
            return;
        }

        messages = messages
            .Where(x => x is not null)
            .ToArray();

        if (messages.Length <= 0)
        {
            return;
        }

        var tasks = messages
            .Select(asyncMessageDispatcher.PublishAsync)
            .ToArray();

        await Task.WhenAll(tasks);
    }
}
