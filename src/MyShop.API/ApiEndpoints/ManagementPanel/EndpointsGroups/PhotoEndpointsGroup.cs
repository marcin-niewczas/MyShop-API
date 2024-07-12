using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.Dtos.ManagementPanel.Photos;
using MyShop.Application.Queries.ManagementPanel.Photos;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Responses;
using MyShop.Infrastructure.Swagger.Operations.ManagementPanel;

namespace MyShop.API.ApiEndpoints.ManagementPanel.EndpointsGroups;

public static class PhotoEndpointsGroup
{
    public static RouteGroupBuilder MapManagementPanelPhotoEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/photos")
           .WithTags("Photos")
           .MapEndpoints();

        return app;
    }

    private static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/website-hero-section-photos", GetPagedWebsiteHeroSectionPhotosAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .WithOpenApi(GetPagedWebsiteHeroSectionPhotosMpOpenApi.ModifyOperation);

        return app;
    }

    private static async Task<Ok<ApiPagedResponse<PhotoMpDto>>> GetPagedWebsiteHeroSectionPhotosAsync(
        [AsParameters] GetPagedWebsiteHeroSectionPhotosMp query,
        [FromServices] IQueryHandler<GetPagedWebsiteHeroSectionPhotosMp, ApiPagedResponse<PhotoMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));
}
