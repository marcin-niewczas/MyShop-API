using MyShop.Application.Abstractions;

namespace MyShop.Application.Events;
public sealed record PriceOfTheProductVariantHasBeenReduced(
    Guid Id
    ) : IEvent;
