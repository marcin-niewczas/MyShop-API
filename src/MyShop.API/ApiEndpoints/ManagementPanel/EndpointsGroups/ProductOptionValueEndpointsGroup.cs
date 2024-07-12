using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands.ManagementPanel.ProductOptions.Details;
using MyShop.Application.Commands.ManagementPanel.ProductOptions.Variants;
using MyShop.Application.Commands.ManagementPanel.ProductOptionValues.Details;
using MyShop.Application.Commands.ManagementPanel.ProductOptionValues.Variants;
using MyShop.Application.Dtos.ManagementPanel.ProductOptionValues;
using MyShop.Application.Responses;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;

namespace MyShop.API.ApiEndpoints.ManagementPanel.EndpointsGroups;

public static class ProductOptionValueEndpointsGroup
{
    public static RouteGroupBuilder MapManagementPanelProductOptionValueEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/product-option-values")
           .WithTags("ProductOptionValues")
           .MapEndpoints();

        return app;
    }

    private static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/details", CreateDetailOptionValueAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        app.MapPut("/details/{id:guid}", UpdateDetailOptionValueAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapDelete("/details/{id:guid}", RemoveDetailOptionValueAsync)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPost("/variants", CreateVariantOptionValueAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        app.MapPut("/variants/{id:guid}", UpdateVariantOptionValueAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapDelete("/variants/{id:guid}", RemoveVariantOptionValueAsync)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<Created<ApiResponse<ProductOptionValueMpDto>>> CreateDetailOptionValueAsync(
       [FromBody] CreateProductDetailOptionValueMp command,
       [FromServices] ICommandHandler<CreateProductDetailOptionValueMp, ApiResponse<ProductOptionValueMpDto>> handler,
       CancellationToken cancellationToken
       ) => TypedResults.Created((string?)null, await handler.HandleAsync(command, cancellationToken));


    private static async Task<Ok<ApiResponse<ProductOptionValueMpDto>>> UpdateDetailOptionValueAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateProductDetailOptionValueMp command,
        [FromServices] ICommandHandler<UpdateProductDetailOptionValueMp, ApiResponse<ProductOptionValueMpDto>> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.Id)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.Id)} in body.");

        var createdResponse = await handler.HandleAsync(command, cancellationToken);
        return TypedResults.Ok(createdResponse);
    }

    private static async Task<NoContent> RemoveDetailOptionValueAsync(
        [AsParameters] RemoveProductDetailOptionValueMp command,
        [FromServices] ICommandHandler<RemoveProductDetailOptionValueMp> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<Created<ApiResponse<ProductOptionValueMpDto>>> CreateVariantOptionValueAsync(
        [FromBody] CreateProductVariantOptionValueMp command,
        [FromServices] ICommandHandler<CreateProductVariantOptionValueMp, ApiResponse<ProductOptionValueMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Created((string?)null, await handler.HandleAsync(command, cancellationToken));

    private static async Task<Ok<ApiResponse<ProductOptionValueMpDto>>> UpdateVariantOptionValueAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateProductVariantOptionValueMp command,
        [FromServices] ICommandHandler<UpdateProductVariantOptionValueMp, ApiResponse<ProductOptionValueMpDto>> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.Id)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.Id)} in body.");

        var createdResponse = await handler.HandleAsync(command, cancellationToken);
        return TypedResults.Ok(createdResponse);
    }

    private static async Task<NoContent> RemoveVariantOptionValueAsync(
        [AsParameters] RemoveProductVariantOptionValueMp command,
        [FromServices] ICommandHandler<RemoveProductVariantOptionValueMp> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }
}
