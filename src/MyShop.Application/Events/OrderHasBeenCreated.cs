using MyShop.Application.Abstractions;

namespace MyShop.Application.Events;
public sealed record OrderHasBeenCreated(
    Guid Id
    ) : IEvent;
