using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.Queries.ECommerce.MainPageSections;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Responses;
using MyShop.Core.Dtos.ECommerce;
using MyShop.Infrastructure.Swagger.Operations.ECommerce;

namespace MyShop.API.ApiEndpoints.ECommerce.EndpointsGroups;

public static class MainPageSectionEndpointsGroup
{
    public static RouteGroupBuilder MapEcommerceMainPageSectionEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/main-page-sections")
           .WithTags("MainPageSections")
           .MapEndpoints();

        return app;
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("/", GetPagedMainPageSectionsAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .WithOpenApi(GetPagedMainPageSectionsEcOpenApi.ModifyOperation);

        return app;
    }

    private static async Task<Ok<ApiPagedResponse<MainPageSectionEcDto>>> GetPagedMainPageSectionsAsync(
        [AsParameters] GetPagedMainPageSectionsEc query,
        [FromServices] IQueryHandler<GetPagedMainPageSectionsEc, ApiPagedResponse<MainPageSectionEcDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));
}
