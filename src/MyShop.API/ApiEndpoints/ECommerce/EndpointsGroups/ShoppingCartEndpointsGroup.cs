using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands.ECommerce.ShoppingCarts;
using MyShop.Application.Dtos.ECommerce.ShoppingCarts;
using MyShop.Application.Responses;

namespace MyShop.API.ApiEndpoints.ECommerce.EndpointsGroups;

public static class ShoppingCartEndpointsGroup
{
    public static RouteGroupBuilder MapEcommerceShoppingCartEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/shopping-carts")
           .WithTags("ShoppingCarts")
           .MapEndpoints()
           .RequireAuthorization();

        return app;
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder app)
    {
        app.MapPost("me/verification", VerifyShoppingCartDetailAsync)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        app.MapPost("me/shopping-cart-items", CreateShoppingCartItemAsync)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        app.MapPut("me/shopping-cart-items", UpdateShoppingCartItemAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        app.MapPatch("me/checkout", CheckoutShoppingCartAsync)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        app.MapDelete("me/shopping-cart-items/{id:guid}", RemoveShoppingCartItemAsync)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<Ok<ApiResponse<ShoppingCartDetailEcDto>>> VerifyShoppingCartDetailAsync(
        [FromServices] ICommandHandler<VerifyShoppingCartEc, ApiResponse<ShoppingCartDetailEcDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(new(), cancellationToken));

    private static async Task<NoContent> CreateShoppingCartItemAsync(
        [FromBody] CreateShoppingCartItemEc command,
        [FromServices] ICommandHandler<CreateShoppingCartItemEc> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<Ok<ApiResponse<ShoppingCartIdValueDictionaryEcDto>>> UpdateShoppingCartItemAsync(
        [FromBody] UpdateShoppingCartItemsEc command,
        [FromServices] ICommandHandler<UpdateShoppingCartItemsEc, ApiResponse<ShoppingCartIdValueDictionaryEcDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(command, cancellationToken));

    [HttpPatch("me/checkout")]
    private static async Task<Ok<ApiResponse<CheckoutEcDto>>> CheckoutShoppingCartAsync(
        [FromServices] ICommandHandler<CheckoutShoppingCartEc, ApiResponse<CheckoutEcDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(new(), cancellationToken));


    [HttpDelete("me/shopping-cart-items/{id:guid}")]
    private static async Task<NoContent> RemoveShoppingCartItemAsync(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<RemoveShoppingCartItemEc> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(new(id), cancellationToken);
        return TypedResults.NoContent();
    }
}
