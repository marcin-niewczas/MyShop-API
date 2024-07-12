using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyShop.Application.Abstractions;
using MyShop.Application.Commands;
using MyShop.Infrastructure.Messaging.Dispatchers.Interfaces;

namespace MyShop.Infrastructure.Messaging;
internal sealed class MessageBackgroundDispatcher(
    IMessageChannel messageChannel,
    IEventDispatcher eventDispatcher,
    ICommandDispatcher commandDispatcher,
    ILogger<MessageBackgroundDispatcher> logger
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Running the background dispatcher.");

        await foreach (var message in messageChannel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await DispatchAsync(message, stoppingToken);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Exception Message: {Message}", exception.Message);
            }
        }

        logger.LogInformation("Finished running the background dispatcher.");
    }

    private Task DispatchAsync(IMessage message, CancellationToken stoppingToken = default)
        => message switch
        {
            IEvent @event => eventDispatcher.PublishAsync(@event, stoppingToken),
            ICommand command => commandDispatcher.HandleAsync(command, stoppingToken),
            _ => throw new NotImplementedException(),
        };
}
