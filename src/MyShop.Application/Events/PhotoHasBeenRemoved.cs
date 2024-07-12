using MyShop.Application.Abstractions;

namespace MyShop.Application.Events;
public sealed record PhotoHasBeenRemoved(
    Guid Id
    ) : IEvent;
