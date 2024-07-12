using MyShop.Application.Abstractions;

namespace MyShop.Application.Events;
public sealed record AuthPasswordHasBeenChanged(
    Guid RegisteredUserId
    ) : IEvent;
