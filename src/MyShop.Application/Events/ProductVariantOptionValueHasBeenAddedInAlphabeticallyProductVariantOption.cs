using MyShop.Application.Abstractions;

namespace MyShop.Application.Events;
public sealed record ProductVariantOptionValueHasBeenAddedInAlphabeticallyProductVariantOption(
    Guid ProductVariantOptionId
    ) : IEvent;
