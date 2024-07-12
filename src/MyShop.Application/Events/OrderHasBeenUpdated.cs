using MyShop.Application.Abstractions;

namespace MyShop.Application.Events;
public sealed record OrderHasBeenUpdated(
    Guid OrderId
    ) : IEvent;
