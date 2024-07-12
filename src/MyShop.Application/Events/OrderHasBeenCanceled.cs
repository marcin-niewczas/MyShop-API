using MyShop.Application.Abstractions;

namespace MyShop.Application.Events;
public sealed record OrderHasBeenCanceled(
    Guid OrderId
    ) : IEvent;
