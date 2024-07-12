using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.Dtos.ManagementPanel.Dashboards;
using MyShop.Application.Queries.ManagementPanel.Dashboards;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Responses;
using MyShop.Infrastructure.Swagger.Operations.ManagementPanel;

namespace MyShop.API.ApiEndpoints.ManagementPanel.EndpointsGroups;

public static class DashboardEndpointsGroup
{
    public static RouteGroupBuilder MapManagementPanelDashboardEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/dashboards")
           .WithTags("Dashboards")
           .MapEndpoints();

        return app;
    }

    private static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", GetPagedDataAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithOpenApi(GetPagedDashboardDataMpOpenApi.ModifyOperation);

        return app;
    }

    private static async Task<Ok<ApiPagedResponse<BaseDashboardElementMpDto>>> GetPagedDataAsync(
        [AsParameters] GetPagedDashboardDataMp query,
        [FromServices] IQueryHandler<GetPagedDashboardDataMp, ApiPagedResponse<BaseDashboardElementMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));
}
