using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands.ECommerce.ProductReviews;
using MyShop.Application.Dtos.ECommerce.ProductReviews;
using MyShop.Application.Dtos.ValidatorParameters.ECommerce;
using MyShop.Application.Responses;
using MyShop.Application.Utils;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;

namespace MyShop.API.ApiEndpoints.ECommerce.EndpointsGroups;

public static class ProductReviewEndpointsGroup
{
    public static RouteGroupBuilder MapEcommerceProductReviewEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/product-reviews")
            .WithTags("ProductReviews")
            .MapEndpoints()
            .RequireAuthorization(PolicyNames.HasCustomerPermission);

        return app;
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder app)
    {
        app.MapPost("/", CreateProductReviewAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        app.MapPut("/{id:guid}", UpdateProductReviewAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapDelete("/{id:guid}", RemoveProductReviewAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("/validator-parameters", GetProductReviewValidatorParametersAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        return app;
    }

    private static async Task<Created<ApiResponse<ProductReviewMeEcDto>>> CreateProductReviewAsync(
        [FromBody] CreateProductReviewEc command,
        [FromServices] ICommandHandler<CreateProductReviewEc, ApiResponse<ProductReviewMeEcDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Created((string?)null, await handler.HandleAsync(command, cancellationToken));

    private static async Task<Ok<ApiResponse<ProductReviewMeEcDto>>> UpdateProductReviewAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateProductReviewEc command,
        [FromServices] ICommandHandler<UpdateProductReviewEc, ApiResponse<ProductReviewMeEcDto>> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.Id)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.Id)} in body.");

        return TypedResults.Ok(await handler.HandleAsync(command, cancellationToken));
    }

    private static async Task<NoContent> RemoveProductReviewAsync(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<RemoveProductReviewEc> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(new(id), cancellationToken);
        return TypedResults.NoContent();
    }

    private static Task<Ok<ApiResponse<ProductReviewValidatorParametersEcDto>>> GetProductReviewValidatorParametersAsync()
        => Task.FromResult(TypedResults.Ok(new ApiResponse<ProductReviewValidatorParametersEcDto>(new())));
}
