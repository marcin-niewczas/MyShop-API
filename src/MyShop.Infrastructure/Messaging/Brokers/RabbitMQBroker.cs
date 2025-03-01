using MassTransit;
using MyShop.Application.Abstractions;

namespace MyShop.Infrastructure.Messaging.Brokers;
internal sealed class RabbitMQBroker(
    IPublishEndpoint publishEndpoint
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
            .Select(m => publishEndpoint.Publish((object)m))
            .ToArray();

        await Task.WhenAll(tasks);
    }
}
