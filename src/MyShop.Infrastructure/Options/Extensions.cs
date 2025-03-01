using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.Options;

namespace MyShop.Infrastructure.Options;
public static class Extensions
{
    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MainDbOptions>(configuration.GetRequiredSection(MainDbOptions.Section));
        services.Configure<AppOptions>(configuration.GetRequiredSection(AppOptions.Section));
        services.Configure<WebSPAClientOptions>(configuration.GetRequiredSection(WebSPAClientOptions.Section));
        services.Configure<AuthOptions>(configuration.GetRequiredSection(AuthOptions.Section));
        services.Configure<MyShopPayOptions>(configuration.GetRequiredSection(MyShopPayOptions.Section));
        services.Configure<MyShopHubOptions>(configuration.GetRequiredSection(MyShopHubOptions.Section));
        services.Configure<MessagingOptions>(configuration.GetRequiredSection(MessagingOptions.Section));

        return services;
    }
}
