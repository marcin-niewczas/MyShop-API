using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.Utils;
using MyShop.Infrastructure.Notifications.Commons;
using MyShop.Infrastructure.Notifications.Orders;
using MyShop.Infrastructure.Notifications.Senders;
using MyShop.Infrastructure.Notifications.Senders.Interfaces;
using System.Reflection;
using static MyShop.Application.Utils.ExtensionsHelper;

namespace MyShop.Infrastructure.Notifications;
internal static class Extensions
{
    public static IServiceCollection AddNotifications(this IServiceCollection services)
    {
        services.AddScoped<IOrderNotificationsSender, OrderNotificationsSender>();
        services.AddScoped<ICommonNotificationsSender, CommonNotificationsSender>();

        var executingAssembly = Assembly.GetExecutingAssembly();

        services.ScanAndRegisterDependenciesAsInterface(typeof(IOrderNotfication), DependencyLifecycle.Scoped, executingAssembly);
        services.ScanAndRegisterDependenciesAsInterface(typeof(ICommonNotification), DependencyLifecycle.Scoped, executingAssembly);

        return services;
    }
}
