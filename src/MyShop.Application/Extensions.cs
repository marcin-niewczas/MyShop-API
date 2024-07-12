using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.ApplicationServices;
using MyShop.Application.Hubs;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Validations;

namespace MyShop.Application;
public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddQueryHandlers()
            .AddValidations()
            .AddApplicationServices()
            .AddHubs();

        return services;
    }

    public static WebApplication UseApplication(this WebApplication app, IConfiguration configuration)
    {
        app.UseHubs(configuration);

        return app;
    }
}
