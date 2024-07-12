using MyShop.Core.Abstractions;
using MyShop.Core.HelperModels;

namespace MyShop.Application.Dtos.ECommerce.ShoppingCarts;
public sealed record ShoppingCartItemDetailEcDto : IDto
{
    public required Guid ShoppingCartItemId { get; init; }
    public required string EncodedName { get; init; }
    public required string FullName { get; init; }
    public required OptionNameValue MainProductVariantOption { get; init; }
    public required IReadOnlyCollection<OptionNameValue> AdditionalProductVariantOptions { get; init; }
    public required Uri? PhotoUrl { get; init; }
    public required string? PhotoAlt { get; init; }
    public required int Quantity { get; init; }
    public required decimal PricePerEach { get; init; }
    public decimal PriceAll => Quantity * PricePerEach;
}
