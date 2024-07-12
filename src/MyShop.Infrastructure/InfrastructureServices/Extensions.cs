using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.Abstractions;

namespace MyShop.Infrastructure.InfrastructureServices;
internal static class Extensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IPhotoFileService, PhotoFileService>();
        services.AddScoped<IIdentifyPlatformService, IdentifyPlatformService>();

        return services;
    }
}
