using Microsoft.Extensions.DependencyInjection;
using MyShop.Infrastructure.Notifications.Commons;
using MyShop.Infrastructure.Notifications.Orders;
using MyShop.Infrastructure.Notifications.Senders;
using MyShop.Infrastructure.Notifications.Senders.Interfaces;
using System.Reflection;

namespace MyShop.Infrastructure.Notifications;
internal static class Extensions
{
    public static IServiceCollection AddNotifications(this IServiceCollection services)
    {
        services.AddScoped<IOrderNotificationsSender, OrderNotificationsSender>();
        services.AddScoped<ICommonNotificationsSender, CommonNotificationsSender>();

        services.Scan(s => s.FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(c => c.AssignableTo(typeof(IOrderNotfication))).AsImplementedInterfaces().WithScopedLifetime()
            );

        services.Scan(s => s.FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(c => c.AssignableTo(typeof(ICommonNotification))).AsImplementedInterfaces().WithScopedLifetime()
            );

        return services;
    }
}
