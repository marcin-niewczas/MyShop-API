using MyShop.Application.Abstractions;

namespace MyShop.Application.Events;
public sealed record ProductVariantOptionSortTypeHasBeenChangedToAlphabetically(
    Guid ProductOptionId
    ) : IEvent;
