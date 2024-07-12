using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.Application.Dtos;
using MyShop.Application.Dtos.ECommerce.Favorites;
using MyShop.Application.Queries.ECommerce.Favorites;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Responses;
using MyShop.Application.Utils;
using MyShop.Infrastructure.Swagger.Operations.ECommerce;

namespace MyShop.API.ApiEndpoints.ECommerce.EndpointsGroups;

public static class FavoriteEndpointsGroup
{
    public static RouteGroupBuilder MapEcommerceFavoriteEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/favorites")
           .WithTags("Favorites")
           .MapEndpoints()
           .RequireAuthorization(PolicyNames.HasCustomerPermission);

        return app;
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("/status", GetStatusOfFavoritesAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithOpenApi(GetStatusOfFavoritesEcOpenApi.ModifyOperation);

        app.MapGet("/{productEncodedName}/status", GetStatusOfFavoriteAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        return app;
    }

    private static async Task<Ok<ApiResponse<StatusOfFavoritesDictionaryEcDto>>> GetStatusOfFavoritesAsync(
       [AsParameters] GetStatusOfFavoritesEc query,
       [FromServices] IQueryHandler<GetStatusOfFavoritesEc, ApiResponse<StatusOfFavoritesDictionaryEcDto>> handler,
       CancellationToken cancellationToken
       ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiResponse<ValueDto<bool>>>> GetStatusOfFavoriteAsync(
       [AsParameters] GetStatusOfFavoriteEc query,
       [FromServices] IQueryHandler<GetStatusOfFavoriteEc, ApiResponse<ValueDto<bool>>> handler,
       CancellationToken cancellationToken
       ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));
}
