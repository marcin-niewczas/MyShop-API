using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands.ManagementPanel.ProductOptions.Details;
using MyShop.Application.Commands.ManagementPanel.ProductOptions.Variants;
using MyShop.Application.Commands.ManagementPanel.ProductOptionValues.Details;
using MyShop.Application.Commands.ManagementPanel.ProductOptionValues.Variants;
using MyShop.Application.Dtos.ManagementPanel.ProductOptions;
using MyShop.Application.Dtos.ManagementPanel.ProductOptionValues;
using MyShop.Application.Dtos.ValidatorParameters.ManagementPanel;
using MyShop.Application.Queries.ManagementPanel.ProductOptions;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Responses;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Infrastructure.Swagger.Operations.ManagementPanel;

namespace MyShop.API.ApiEndpoints.ManagementPanel.EndpointsGroups;

public static class ProductOptionEndpointsGroup
{
    public static RouteGroupBuilder MapManagementPanelProductOptionsEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/product-options")
           .WithTags("ProductOptions")
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
            .WithOpenApi(GetPagedProductOptionsMpOpenApi.ModifyOperation);

        app.MapGet("/{id:guid}", GetByIdAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("/{id:guid}/product-option-values", GetPagedProductOptionValuesByProductOptionIdAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi(GetPagedProductOptionValuesByProductOptionIdMpOpenApi.ModifyOperation);

        app.MapPost("/details", CreateDetailOptionAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        app.MapPut("/details/{id:guid}", UpdateDetailOptionAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapDelete("/details/{id:guid}", RemoveDetailOptionAsync)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPut("/details/{id:guid}/product-detail-option-values", UpdatePositionsOfProductDetailOptionValuesAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPost("/variants", CreateVariantOptionAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        app.MapPut("/variants/{id:guid}", UpdateVariantOptionAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapDelete("/variants/{id:guid}", RemoveVariantOptionAsync)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPut("/variants/{id:guid}/product-variant-option-values", UpdatePositionsOfProductVariantOptionValuesAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("/validator-parameters", GetProductOptionValidatorParametersAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        return app;
    }

    private static async Task<Ok<ApiPagedResponse<ProductOptionMpDto>>> GetPagedDataAsync(
        [AsParameters] GetPagedProductOptionsMp query,
        [FromServices] IQueryHandler<GetPagedProductOptionsMp, ApiPagedResponse<ProductOptionMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiResponse<ProductOptionMpDto>>> GetByIdAsync(
        [AsParameters] GetProductOptionByIdMp query,
        [FromServices] IQueryHandler<GetProductOptionByIdMp, ApiResponse<ProductOptionMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiPagedResponse<ProductOptionValueMpDto>>> GetPagedProductOptionValuesByProductOptionIdAsync(
        [AsParameters] GetPagedProductOptionValuesByProductOptionIdMp query,
        [FromServices] IQueryHandler<GetPagedProductOptionValuesByProductOptionIdMp, ApiPagedResponse<ProductOptionValueMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Created<ApiIdResponse>> CreateDetailOptionAsync(
        [FromBody] CreateProductDetailOptionMp command,
        [FromServices] ICommandHandler<CreateProductDetailOptionMp, ApiIdResponse> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Created((string?)null, await handler.HandleAsync(command, cancellationToken));

    private static async Task<Ok<ApiResponse<ProductOptionMpDto>>> UpdateDetailOptionAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateProductDetailOptionMp command,
        [FromServices] ICommandHandler<UpdateProductDetailOptionMp, ApiResponse<ProductOptionMpDto>> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.Id)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.Id)} in body.");

        return TypedResults.Ok(await handler.HandleAsync(command, cancellationToken));
    }

    private static async Task<NoContent> RemoveDetailOptionAsync(
        [AsParameters] RemoveProductDetailOptionMp command,
        [FromServices] ICommandHandler<RemoveProductDetailOptionMp> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> UpdatePositionsOfProductDetailOptionValuesAsync(
        [FromRoute] Guid id,
        [FromBody] UpdatePositionsOfProductDetailOptionValuesMp command,
        [FromServices] ICommandHandler<UpdatePositionsOfProductDetailOptionValuesMp> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.ProductDetailOptionId)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.ProductDetailOptionId)} in body.");

        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }

    [HttpPost("variants")]
    private static async Task<Created<ApiIdResponse>> CreateVariantOptionAsync(
        [FromBody] CreateProductVariantOptionMp command,
        [FromServices] ICommandHandler<CreateProductVariantOptionMp, ApiIdResponse> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Created((string?)null, await handler.HandleAsync(command, cancellationToken));

    private static async Task<Ok<ApiResponse<ProductOptionMpDto>>> UpdateVariantOptionAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateProductVariantOptionsMp command,
        [FromServices] ICommandHandler<UpdateProductVariantOptionsMp, ApiResponse<ProductOptionMpDto>> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.Id)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.Id)} in body.");

        return TypedResults.Ok(await handler.HandleAsync(command, cancellationToken));
    }

    private static async Task<NoContent> RemoveVariantOptionAsync(
        [AsParameters] RemoveProductVariantOptionMp command,
        [FromServices] ICommandHandler<RemoveProductVariantOptionMp> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> UpdatePositionsOfProductVariantOptionValuesAsync(
        [FromRoute] Guid id,
        [FromBody] UpdatePositionsOfProductVariantOptionValuesMp command,
        [FromServices] ICommandHandler<UpdatePositionsOfProductVariantOptionValuesMp> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.ProductVariantOptionId)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.ProductVariantOptionId)} in body.");

        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static Task<Ok<ApiResponse<ProductOptionValidatorParametersMpDto>>> GetProductOptionValidatorParametersAsync()
        => Task.FromResult(TypedResults.Ok(new ApiResponse<ProductOptionValidatorParametersMpDto>(new())));
}
