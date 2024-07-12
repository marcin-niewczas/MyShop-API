using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands.ManagementPanel.ProductProductDetailOptionValues;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;

namespace MyShop.API.ApiEndpoints.ManagementPanel.EndpointsGroups;

public static class ProductProductDetailOptionValueEndpointsGroup
{
    public static RouteGroupBuilder MapManagementPanelProductProductDetailOptionValueEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/product-product-detail-option-values")
           .WithTags("ProductProductDetailOptionValues")
           .MapEndpoints();

        return app;
    }

    private static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", CreateProductProductDetailOptionValueAsync)
           .ProducesValidationProblem()
           .ProducesProblem(StatusCodes.Status401Unauthorized)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPatch("/{id:guid}", UpdateProductProductDetailOptionValueAsync)
           .ProducesValidationProblem()
           .ProducesProblem(StatusCodes.Status401Unauthorized)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapDelete("/{id:guid}", RemoveProductProductDetailOptionValueAsync)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }


    private static async Task<NoContent> CreateProductProductDetailOptionValueAsync(
        [FromBody] CreateProductProductDetailOptionValueMp command,
        [FromServices] ICommandHandler<CreateProductProductDetailOptionValueMp> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> UpdateProductProductDetailOptionValueAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateProductProductDetailOptionValueMp command,
        [FromServices] ICommandHandler<UpdateProductProductDetailOptionValueMp> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.Id)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.Id)} in body.");

        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> RemoveProductProductDetailOptionValueAsync(
        [AsParameters] RemoveProductProductDetailOptionValueMp command,
        [FromServices] ICommandHandler<RemoveProductProductDetailOptionValueMp> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }
}
