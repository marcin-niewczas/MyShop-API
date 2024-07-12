using Microsoft.AspNetCore.Http.HttpResults;
using MyShop.Application.Dtos.Configurations;
using MyShop.Application.Responses;

namespace MyShop.API.ApiEndpoints.Configuration;

public static class ConfigurationEndpointsGroup
{
    public static RouteGroupBuilder MapConfigurationEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/configurations")
           .WithTags("Configurations")
           .MapConfigurationEndpoints();

        return app;
    }

    private static IEndpointRouteBuilder MapConfigurationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/shared", GetSharedConfigurationAsync);

        return app;
    }

    private static async Task<Ok<ApiResponse<SharedConfigurationDto>>> GetSharedConfigurationAsync()
        => await Task.FromResult(TypedResults.Ok(new ApiResponse<SharedConfigurationDto>(new())));
}
