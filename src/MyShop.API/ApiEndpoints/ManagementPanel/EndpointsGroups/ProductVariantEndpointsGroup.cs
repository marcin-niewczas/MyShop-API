using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands.ManagementPanel.ProductVariants;
using MyShop.Application.Dtos.ManagementPanel.ProductVariantPhotoItems;
using MyShop.Application.Dtos.ValidatorParameters.ManagementPanel;
using MyShop.Application.Queries.ManagementPanel.ProductVariantPhotoItems;
using MyShop.Application.Queries.ManagementPanel.ProductVariants;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Responses;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;

namespace MyShop.API.ApiEndpoints.ManagementPanel.EndpointsGroups;

public static class ProductVariantEndpointsGroup
{
    public static RouteGroupBuilder MapManagementPanelProductVariantEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/product-variants")
           .WithTags("ProductVariants")
           .MapEndpoints();

        return app;
    }

    private static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("{id:guid}", GetProductVariantAsync)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPut("{id:guid}", UpdateProductVariantAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("{id:guid}/product-variant-photo-items", GetProductVariantPhotosAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        app.MapPost("{id:guid}/product-variant-photo-items/upload", UploadProductVariantPhotosAsync)
            .DisableAntiforgery()
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPost("{id:guid}/product-variant-photo-items", CreateProductVariantPhotoItemsAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapDelete("/product-variant-photo-items/{id:guid}", RemoveProductVariantPhotoAsync)
           .ProducesProblem(StatusCodes.Status401Unauthorized)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPatch("/{id:guid}/product-variant-photo-items/positions", UpdatePositionsOfProductVariantPhotosAsync)
           .AddEndpointFilter<ModelValidateEndpointFilter>()
           .ProducesProblem(StatusCodes.Status401Unauthorized)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("/validator-parameters", GetProductVariantValidatorParametersAsync)
           .ProducesProblem(StatusCodes.Status401Unauthorized)
           .ProducesProblem(StatusCodes.Status403Forbidden);

        return app;
    }

    private static async Task<Ok<ApiResponse<ProductVariantMpDto>>> GetProductVariantAsync(
        [AsParameters] GetProductVariantMp query,
        [FromServices] IQueryHandler<GetProductVariantMp, ApiResponse<ProductVariantMpDto>> handler
        ) => TypedResults.Ok(await handler.HandleAsync(query));

    private static async Task<NoContent> UpdateProductVariantAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateProductVariantMp command,
        [FromServices] ICommandHandler<UpdateProductVariantMp> handler
        )
    {
        if (id != command.Id)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.Id)} in body.");

        await handler.HandleAsync(command);
        return TypedResults.NoContent();
    }

    private static async Task<Ok<ApiResponseWithCollection<ProductVariantPhotoItemMpDto>>> GetProductVariantPhotosAsync(
        [AsParameters] GetProductVariantPhotoItemsMp query,
        [FromServices] IQueryHandler<GetProductVariantPhotoItemsMp, ApiResponseWithCollection<ProductVariantPhotoItemMpDto>> handler
        ) => TypedResults.Ok(await handler.HandleAsync(query));



    private static async Task<Created> CreateProductVariantPhotoItemsAsync(
       [FromRoute] Guid id,
       [FromBody] CreateProductVariantPhotoItemsMp command,
       [FromServices] ICommandHandler<CreateProductVariantPhotoItemsMp> handler
       )
    {
        if (id != command.Id)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.Id)} in body.");

        await handler.HandleAsync(command);
        return TypedResults.Created();
    }
    private static async Task<Created> UploadProductVariantPhotosAsync(
        [AsParameters] UploadProductVariantPhotoMp command,
        [FromServices] ICommandHandler<UploadProductVariantPhotoMp> handler
        )
    {
        await handler.HandleAsync(command);
        return TypedResults.Created();
    }

    private static async Task<NoContent> RemoveProductVariantPhotoAsync(
        [AsParameters] RemoveProductVariantPhotoItemsMp command,
        [FromServices] ICommandHandler<RemoveProductVariantPhotoItemsMp> handler
        )
    {
        await handler.HandleAsync(command);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> UpdatePositionsOfProductVariantPhotosAsync(
        [AsParameters] UpdatePositionsOfProductVariantPhotoItemsMp command,
        [FromServices] ICommandHandler<UpdatePositionsOfProductVariantPhotoItemsMp> handler
        )
    {
        await handler.HandleAsync(command);
        return TypedResults.NoContent();
    }

    private static Task<Ok<ApiResponse<ProductVariantValidatorParametersMpDto>>> GetProductVariantValidatorParametersAsync()
        => Task.FromResult(TypedResults.Ok(new ApiResponse<ProductVariantValidatorParametersMpDto>(new())));
}
