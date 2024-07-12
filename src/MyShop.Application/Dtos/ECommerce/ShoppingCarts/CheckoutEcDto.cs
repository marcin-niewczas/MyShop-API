using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.ECommerce.ShoppingCarts;
public sealed record CheckoutEcDto : IDto
{
    public required string? CheckoutId { get; init; }
}
