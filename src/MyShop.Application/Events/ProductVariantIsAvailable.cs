using MyShop.Application.Abstractions;

namespace MyShop.Application.Events;
public sealed record ProductVariantIsAvailable(
    Guid Id
    ) : IEvent;
