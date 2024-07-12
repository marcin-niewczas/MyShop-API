using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.Hubs.Providers;
using MyShop.Application.Hubs.Shared;
using MyShop.Application.Options;

namespace MyShop.Application.Hubs;
internal static class Extensions
{
    public static IServiceCollection AddHubs(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddSingleton<IUserIdProvider, NameBasedUserIdProvider>();

        return services;
    }

    public static WebApplication UseHubs(this WebApplication app, IConfiguration configuration)
    {
        var hubOptions = configuration.GetOptions<MyShopHubOptions>(MyShopHubOptions.Section);

        app.MapHub<NotificationsHub>($"{hubOptions.SharedPath}/notifications");

        return app;
    }
}
