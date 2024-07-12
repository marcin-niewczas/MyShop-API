using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands.ManagementPanel.ProductReviews;

namespace MyShop.API.ApiEndpoints.ManagementPanel.EndpointsGroups;

public static class ProductReviewEndpointsGroup
{
    public static RouteGroupBuilder MapManagementPanelProductReviewEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/product-reviews")
           .WithTags("ProductReviews")
           .MapEndpoints();

        return app;
    }

    private static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id:guid}", RemoveProductReviewAsync)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);



        return app;
    }

    private static async Task<NoContent> RemoveProductReviewAsync(
        [AsParameters] RemoveProductReviewMp command,
        [FromServices] ICommandHandler<RemoveProductReviewMp> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }
}
