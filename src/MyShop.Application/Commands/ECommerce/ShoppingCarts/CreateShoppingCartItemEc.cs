namespace MyShop.Application.Commands.ECommerce.ShoppingCarts;
public sealed record CreateShoppingCartItemEc(
    Guid ProductVariantId
    ) : ICommand;
