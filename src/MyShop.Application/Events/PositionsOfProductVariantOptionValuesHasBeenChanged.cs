using MyShop.Application.Abstractions;
using MyShop.Core.HelperModels;

namespace MyShop.Application.Events;
public sealed record PositionsOfProductVariantOptionValuesHasBeenChanged(
    IReadOnlyCollection<ValuePosition<Guid>> PositionOfProductOptionValue
    ) : IEvent;
