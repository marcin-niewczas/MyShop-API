using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.Application.Queries.Account.Favorites;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Responses;
using MyShop.Core.Dtos.Shared;
using MyShop.Infrastructure.Swagger.Operations.Account;

namespace MyShop.API.ApiEndpoints.Account.EndpointsGroups;

public static class FavoriteEndpointsGroup
{
    public static RouteGroupBuilder MapAccountFavoriteEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("favorites")
            .WithTags("Favorites")
            .MapEndpoints();

        return app;
    }

    private static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", GetPagedFavoritesAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesValidationProblem()
            .WithOpenApi(GetPagedFavoritesAcOpenApi.ModifyOperation);

        return app;
    }

    private static async Task<Ok<ApiPagedResponse<ProductItemDto>>> GetPagedFavoritesAsync(
       [AsParameters] GetPagedFavoritesAc query,
       [FromServices] IQueryHandler<GetPagedFavoritesAc, ApiPagedResponse<ProductItemDto>> handler,
       CancellationToken cancellationToken
       ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));
}
