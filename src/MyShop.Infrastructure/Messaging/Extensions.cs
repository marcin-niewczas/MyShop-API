using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.Abstractions;
using MyShop.Infrastructure.Messaging.Brokers;
using MyShop.Infrastructure.Messaging.Dispatchers;
using MyShop.Infrastructure.Messaging.Dispatchers.Interfaces;
using System.Reflection;

namespace MyShop.Infrastructure.Messaging;
internal static class Extensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddHostedService<MessageBackgroundDispatcher>();
        services.AddSingleton<IMessageChannel, MessageChannel>();
        services.AddSingleton<IEventDispatcher, EventDispatcher>();
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.AddSingleton<IAsyncMessageDispatcher, AsyncMessageDispatcher>();
        services.AddSingleton<IMessageBroker, MessageBroker>();

        services.Scan(s => s.FromAssemblies(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(Application.Extensions))!)
            .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>))).AsImplementedInterfaces().WithScopedLifetime()
            );

        return services;
    }
}
