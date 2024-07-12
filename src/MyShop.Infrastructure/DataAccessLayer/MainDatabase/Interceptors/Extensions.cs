using Microsoft.Extensions.DependencyInjection;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Interceptors;
internal static class Extensions
{
    public static IServiceCollection AddInterceptors(this IServiceCollection services)
    {
        services.AddSingleton<MainDbSaveChangesInterceptor>();

        return services;
    }
}
