using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.ApplicationServices.Interfaces;

namespace MyShop.Application.ApplicationServices;
internal static class Extensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDashboardService, DashboardService>();

        return services;
    }
}
