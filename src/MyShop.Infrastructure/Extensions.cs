using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyShop.Infrastructure.Commands;
using MyShop.Infrastructure.CronJobs;
using MyShop.Infrastructure.DataAccessLayer;
using MyShop.Infrastructure.Exceptions;
using MyShop.Infrastructure.InfrastructureServices;
using MyShop.Infrastructure.Messaging;
using MyShop.Infrastructure.Notifications;
using MyShop.Infrastructure.Options;
using MyShop.Infrastructure.Payments;
using MyShop.Infrastructure.Security;
using MyShop.Infrastructure.Swagger;

namespace MyShop.Infrastructure;
public static class Extensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IWebHostEnvironment environment,
        IConfiguration configuration
        )
    {
        services
            .AddErrorHandling()
            .AddOptions(configuration)
            .AddSecurity(configuration)
            .AddDataAccessLayer(environment)
            .AddInfrastructureServices()
            .AddSwaggerExtension()
            .AddHttpContextAccessor()
            .AddMessaging(configuration)
            .AddNotifications()
            .AddHttpClient()
            .AddPayments()
            .AddCronJobs(configuration)
            .AddCommandHandlers();

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app, IConfiguration configuration)
    {
        app.UseErrorHandling();
        app.UseSecurity(configuration);
        app.UseSwaggerExtension();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        return app;
    }
}
