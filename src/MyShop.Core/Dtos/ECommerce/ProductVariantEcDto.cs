using MyShop.Core.Abstractions;

namespace MyShop.Core.Dtos.ECommerce;
public sealed record ProductVariantEcDto(
    Guid ProductVariantId,
    string VariantLabel,
    decimal Price,
    bool? LastItemsInStock
    ) : IDto;
