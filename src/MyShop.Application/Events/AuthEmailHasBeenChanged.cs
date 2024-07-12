using MyShop.Application.Abstractions;

namespace MyShop.Application.Events;
public sealed record AuthEmailHasBeenChanged(
    Guid RegisteredUserId
    ) : IEvent;
