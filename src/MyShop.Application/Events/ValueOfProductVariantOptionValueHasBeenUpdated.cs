using MyShop.Application.Abstractions;

namespace MyShop.Application.Events;
public sealed record ValueOfProductVariantOptionValueHasBeenUpdated(
    Guid ProductVariantOptionValueId
    ) : IEvent;
