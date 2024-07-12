using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.ECommerce.ShoppingCarts;
public sealed record ShoppingCartDetailEcDto : IDto
{
    public int TotalQuantity
        => ShoppingCartItems.Sum(i => i.Quantity);
    public decimal TotalPrice
        => ShoppingCartItems.Sum(i => i.PriceAll);
    public required IReadOnlyDictionary<Guid, ShoppingCartItemChangedEcDto>? Changes { get; init; }
    public required IReadOnlyCollection<ShoppingCartItemDetailEcDto> ShoppingCartItems { get; init; }
}

public sealed record ShoppingCartItemChangedEcDto(int From, int To, string ProductName);
