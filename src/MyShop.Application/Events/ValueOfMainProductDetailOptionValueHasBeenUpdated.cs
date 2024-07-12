using MyShop.Application.Abstractions;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Application.Events;
public sealed record ValueOfMainProductDetailOptionValueHasBeenUpdated(
    Guid ProductDetailOptionValueId,
    ProductOptionValue OldValue
    ) : IEvent;
