using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MyShop.Infrastructure.DataAccessLayer.MainDatabase;

namespace MyShop.Infrastructure.DataAccessLayer;
internal static class Extensions
{
    public static IServiceCollection AddDataAccessLayer(
        this IServiceCollection services,
        IWebHostEnvironment environment
        )
    {
        services.AddMainDatabase(environment);

        return services;
    }
}
