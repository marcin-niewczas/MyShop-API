using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyShop.Application.CommandHandlers;
using MyShop.Infrastructure.Logging.Decorators;
using System.Reflection;

namespace MyShop.Infrastructure.Commands;
internal static class Extensions
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services.RegisterAndDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>), Assembly.GetAssembly(typeof(Application.Extensions)));
        services.RegisterAndDecorate(typeof(ICommandHandler<,>), typeof(LoggingCommandHandlerDecorator<,>), Assembly.GetAssembly(typeof(Application.Extensions)));

        services.RegisterAndDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        services.RegisterAndDecorate(typeof(ICommandHandler<,>), typeof(LoggingCommandHandlerDecorator<,>));

        return services;
    }

    private static IServiceCollection RegisterAndDecorate(
        this IServiceCollection services,
        Type serviceType,
        Type decoratorType,
        Assembly? assembly = null
        )
    {
        assembly ??= Assembly.GetExecutingAssembly();

        var handlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == serviceType)
                .Select(i => new { Interface = i, Implementation = t }))
            .ToList();

        foreach (var handler in handlerTypes)
        {
            services.AddScoped(handler.Implementation);

            services.AddScoped(handler.Interface, serviceProvider =>
            {
                var innerHandler = serviceProvider.GetRequiredService(handler.Implementation);
                var loggerType = typeof(ILogger<>).MakeGenericType(decoratorType.MakeGenericType(handler.Interface.GenericTypeArguments));
                var logger = serviceProvider.GetRequiredService(loggerType);

                var objType = decoratorType.MakeGenericType(handler.Interface.GenericTypeArguments);
                return Activator.CreateInstance(objType, innerHandler, logger)!;
            });
        }

        return services;
    }
}
