namespace MyShop.Application.Commands.ECommerce.ShoppingCarts;
public sealed record RemoveShoppingCartItemEc(
    Guid Id
    ) : ICommand;
