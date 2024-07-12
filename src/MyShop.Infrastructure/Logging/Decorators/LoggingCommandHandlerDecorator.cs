using Microsoft.Extensions.Logging;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands;

namespace MyShop.Infrastructure.Logging.Decorators;
internal sealed class LoggingCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> commandHandler,
    ILogger<LoggingCommandHandlerDecorator<TCommand>> logger
    ) : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        var commandName = typeof(TCommand).Name;

        logger.LogInformation("Started handling a command: {CommandName}...", commandName);

        await commandHandler.HandleAsync(command, cancellationToken);

        logger.LogInformation("Completed handling a command: {CommandName}.", commandName);
    }
}

internal sealed class LoggingCommandHandlerDecorator<TCommand, TResult>(
    ICommandHandler<TCommand, TResult> commandHandler,
    ILogger<LoggingCommandHandlerDecorator<TCommand, TResult>> logger
    ) : ICommandHandler<TCommand, TResult> where TCommand : class, ICommand<TResult>
{
    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        var commandName = typeof(TCommand).Name;

        logger.LogInformation("Started handling a command: {CommandName}...", commandName);

        var result = await commandHandler.HandleAsync(command, cancellationToken);

        logger.LogInformation("Completed handling a command: {CommandName}.", commandName);

        return result;
    }
}
