using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands.ManagementPanel.Products;
using MyShop.Application.Commands.ManagementPanel.ProductVariants;
using MyShop.Application.Dtos.ManagementPanel.Photos;
using MyShop.Application.Dtos.ManagementPanel.ProductReviews;
using MyShop.Application.Dtos.ManagementPanel.Products;
using MyShop.Application.Dtos.ValidatorParameters.ManagementPanel;
using MyShop.Application.Queries.ManagementPanel.Products;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Responses;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Infrastructure.Swagger.Operations.ECommerce;
using MyShop.Infrastructure.Swagger.Operations.ManagementPanel;

namespace MyShop.API.ApiEndpoints.ManagementPanel.EndpointsGroups;

public static class ProductEndpointsGroup
{
    public static RouteGroupBuilder MapManagementPanelProductEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/products")
           .WithTags("Products")
           .MapEndpoints();

        return app;
    }

    private static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", GetPagedDataAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .WithOpenApi(GetPagedProductsMpOpenApi.ModifyOperation);

        app.MapPost("/", CreateProductAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        app.MapGet("/{id:guid}", GetByIdAsync)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPut("/{id:guid}", UpdateProductAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("/{id:guid}/photos", GetPagedProductPhotosByProductIdAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi(GetPagedProductPhotosMpOpenApi.ModifyOperation);

        app.MapGet("/{id:guid}/product-variants", GetPagedProductVariantsByProductIdAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi(GetPagedProductVariantsByProductIdMpOpenApi.ModifyOperation);

        app.MapPost("/{id:guid}/product-variants", CreateProductVariantAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("/{id:guid}/product-detail-options", GetPagedProductDetailOptionsByProductIdAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .WithOpenApi(GetPagedProductDetailOptionsByProductIdMpOpenApi.ModifyOperation);

        app.MapPatch("/{id:guid}/product-detail-options/positions", UpdateProductDetailOptionsPositionsOfProductAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPatch("/{id:guid}/product-variant-options/positions", UpdateProductVariantOptionsPositionsOfProductAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("/{id:guid}/product-variant-options", GetPagedProductVariantOptionsByProductIdAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi(GetPagedProductVariantOptionsByProductIdMpOpenApi.ModifyOperation);

        app.MapGet("/{id:guid}/product-reviews", GetPagedProductReviewsAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi(GetPagedProductReviewsEcOpenApi.ModifyOperation);

        app.MapGet("/validator-parameters", GetProductValidatorParametersAsync)
           .ProducesProblem(StatusCodes.Status401Unauthorized)
           .ProducesProblem(StatusCodes.Status403Forbidden);

        return app;
    }

    private static async Task<Ok<ApiPagedResponse<PagedProductMpDto>>> GetPagedDataAsync(
        [AsParameters] GetPagedProductsMp query,
        [FromServices] IQueryHandler<GetPagedProductsMp, ApiPagedResponse<PagedProductMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Created<ApiIdResponse>> CreateProductAsync(
        [FromBody] CreateProductMp command,
        [FromServices] ICommandHandler<CreateProductMp, ApiIdResponse> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Created((string?)null, await handler.HandleAsync(command, cancellationToken));

    private static async Task<Ok<ApiResponse<ProductMpDto>>> GetByIdAsync(
        [AsParameters] GetProductMp query,
        [FromServices] IQueryHandler<GetProductMp, ApiResponse<ProductMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<NoContent> UpdateProductAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateProductMp command,
        [FromServices] ICommandHandler<UpdateProductMp> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.Id)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.Id)} in body.");

        await handler.HandleAsync(command, cancellationToken);

        return TypedResults.NoContent();
    }

    private static async Task<Ok<ApiPagedResponse<PhotoMpDto>>> GetPagedProductPhotosByProductIdAsync(
        [AsParameters] GetPagedProductPhotosMp query,
        [FromServices] IQueryHandler<GetPagedProductPhotosMp, ApiPagedResponse<PhotoMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiPagedResponse<PagedProductVariantMpDto>>> GetPagedProductVariantsByProductIdAsync(
        [AsParameters] GetPagedProductVariantsByProductIdMp query,
        [FromServices] IQueryHandler<GetPagedProductVariantsByProductIdMp, ApiPagedResponse<PagedProductVariantMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Created<ApiIdResponse>> CreateProductVariantAsync(
        [FromRoute] Guid id,
        [FromBody] CreateProductVariantMp command,
        [FromServices] ICommandHandler<CreateProductVariantMp, ApiIdResponse> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.ProductId)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.ProductId)} in body.");

        return TypedResults.Created((string?)null, await handler.HandleAsync(command, cancellationToken));
    }

    private static async Task<Ok<ApiPagedResponse<ProductDetailOptionOfProductMpDto>>> GetPagedProductDetailOptionsByProductIdAsync(
        [AsParameters] GetPagedProductDetailOptionsByProductIdMp query,
        [FromServices] IQueryHandler<GetPagedProductDetailOptionsByProductIdMp, ApiPagedResponse<ProductDetailOptionOfProductMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<NoContent> UpdateProductDetailOptionsPositionsOfProductAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateProductDetailOptionsPositionsOfProductMp command,
        [FromServices] ICommandHandler<UpdateProductDetailOptionsPositionsOfProductMp> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.ProductId)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.ProductId)} in body.");

        await handler.HandleAsync(command, cancellationToken);

        return TypedResults.NoContent();
    }

    private static async Task<NoContent> UpdateProductVariantOptionsPositionsOfProductAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateProductVariantOptionsPositionsOfProductMp command,
        [FromServices] ICommandHandler<UpdateProductVariantOptionsPositionsOfProductMp> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.ProductId)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.ProductId)} in body.");

        await handler.HandleAsync(command, cancellationToken);

        return TypedResults.NoContent();
    }

    private static async Task<Ok<ApiPagedResponse<ProductVariantOptionOfProductMpDto>>> GetPagedProductVariantOptionsByProductIdAsync(
        [AsParameters] GetPagedProductVariantOptionsByProductIdMp query,
        [FromServices] IQueryHandler<GetPagedProductVariantOptionsByProductIdMp, ApiPagedResponse<ProductVariantOptionOfProductMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiPagedResponse<ProductReviewMpDto>>> GetPagedProductReviewsAsync(
        [AsParameters] GetPagedProductReviewsByProductIdMp query,
        [FromServices] IQueryHandler<GetPagedProductReviewsByProductIdMp, ApiPagedResponse<ProductReviewMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static Task<Ok<ApiResponse<ProductValidatorParametersMpDto>>> GetProductValidatorParametersAsync()
        => Task.FromResult(TypedResults.Ok(new ApiResponse<ProductValidatorParametersMpDto>(new())));
}
