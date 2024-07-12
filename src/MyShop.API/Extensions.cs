using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json.Serialization;

namespace MyShop.API;

internal static class Extensions
{
    public static IServiceCollection AddApi(this IServiceCollection services, ConfigureHostBuilder host)
    {
        host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
        });

        services.Configure<JsonOptions>((config) =>
        {
            config.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services
            .AddOutputCache()
            .AddCors();

        services.AddHealthChecks();

        return services;
    }
}
