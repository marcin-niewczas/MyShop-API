using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands.ECommerce.Products;
using MyShop.Application.Dtos;
using MyShop.Application.Dtos.ECommerce.ProductReviews;
using MyShop.Application.Queries.ECommerce.Products;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Responses;
using MyShop.Application.Utils;
using MyShop.Core.Dtos.ECommerce;
using MyShop.Core.Dtos.Shared;
using MyShop.Infrastructure.Swagger.Operations.ECommerce;

namespace MyShop.API.ApiEndpoints.ECommerce.EndpointsGroups;

public static class ProductEndpointsGroup
{
    public static RouteGroupBuilder MapEcommerceProductEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/products")
            .WithTags("Products")
            .MapEndpoints();

        return app;
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("/", GetPagedDataProductsAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .WithOpenApi(GetPagedProductsEcOpenApi.ModifyOperation);

        app.MapGet("/names", GetProductsNamesAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .WithOpenApi(GetProductNamesEcOpenApi.ModifyOperation);

        app.MapGet("/{encodedName}", GetProductDetailAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPost("/{encodedName}/favorites", CreateFavoriteAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.HasCustomerPermission);

        app.MapDelete("/{encodedName}/favorites", RemoveFavoriteAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.HasCustomerPermission);

        app.MapGet("/{encodedName}/product-variants", GetProductVariantsAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem();

        app.MapGet("/{id:guid}/product-reviews", GetPagedProductReviewsAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi(GetPagedProductReviewsEcOpenApi.ModifyOperation);

        app.MapGet("/{id:guid}/product-reviews/me", GetProductReviewMeByProductIdAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.HasCustomerPermission);

        app.MapGet("/{id:guid}/product-reviews/rate-stats", GetProductReviewRateStatsAsync)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<Ok<ApiPagedResponse<ProductItemDto>>> GetPagedDataProductsAsync(
        [AsParameters] GetPagedProductsEc query,
        [FromServices] IQueryHandler<GetPagedProductsEc, ApiPagedResponse<ProductItemDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiResponse<ValueDto<IReadOnlyCollection<string>>>>> GetProductsNamesAsync(
        [AsParameters] GetPagedProductsNamesEc query,
        [FromServices] IQueryHandler<GetPagedProductsNamesEc, ApiResponse<ValueDto<IReadOnlyCollection<string>>>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiResponse<BaseProductDetailEcDto>>> GetProductDetailAsync(
        [AsParameters] GetProductEc query,
        [FromServices] IQueryHandler<GetProductEc, ApiResponse<BaseProductDetailEcDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Created> CreateFavoriteAsync(
        [AsParameters] CreateFavoriteEc query,
        [FromServices] ICommandHandler<CreateFavoriteEc> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(query, cancellationToken);
        return TypedResults.Created();
    }

    private static async Task<NoContent> RemoveFavoriteAsync(
        [AsParameters] RemoveFavoriteEc query,
        [FromServices] ICommandHandler<RemoveFavoriteEc> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(query, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<Ok<ApiResponseWithCollection<ProductVariantEcDto>>> GetProductVariantsAsync(
       [AsParameters] GetProductVariantsEc query,
       [FromServices] IQueryHandler<GetProductVariantsEc, ApiResponseWithCollection<ProductVariantEcDto>> handler,
       CancellationToken cancellationToken
       ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiPagedResponse<ProductReviewEcDto>>> GetPagedProductReviewsAsync(
        [AsParameters] GetPagedProductReviewsEc query,
        [FromServices] IQueryHandler<GetPagedProductReviewsEc, ApiPagedResponse<ProductReviewEcDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiResponse<ProductReviewMeEcDto>>> GetProductReviewMeByProductIdAsync(
        [AsParameters] GetProductReviewMeByProductIdEc query,
        [FromServices] IQueryHandler<GetProductReviewMeByProductIdEc, ApiResponse<ProductReviewMeEcDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiResponse<ProductReviewRateStatEcDto>>> GetProductReviewRateStatsAsync(
        [AsParameters] GetProductReviewRateStatsEc query,
        [FromServices] IQueryHandler<GetProductReviewRateStatsEc, ApiResponse<ProductReviewRateStatEcDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));
}
