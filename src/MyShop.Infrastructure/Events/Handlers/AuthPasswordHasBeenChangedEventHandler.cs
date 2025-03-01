using MassTransit;
using MyShop.Application.Abstractions;
using MyShop.Application.Events;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Notifications;
using MyShop.Core.ValueObjects.Notifications;
using MyShop.Infrastructure.Notifications.Senders.Interfaces;

namespace MyShop.Infrastructure.Events.Handlers;
internal sealed class AuthPasswordHasBeenChangedEventHandler(
    IUnitOfWork unitOfWork,
    ICommonNotificationsSender commonNotificationsSender
    ) : IEventHandler<AuthPasswordHasBeenChanged>, 
        IConsumer<AuthPasswordHasBeenChanged>
{
    public async Task HandleAsync(AuthPasswordHasBeenChanged @event, CancellationToken cancellationToken = default)
    {
        var notification = new Notification(
            NotificationType.Security,
            "Your Password has been changed.",
            @event.RegisteredUserId
            );

        await unitOfWork.AddAsync(notification, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await commonNotificationsSender.SendAsync(
            @event.RegisteredUserId,
            notification,
            cancellationToken: cancellationToken
            );
    }

    public Task Consume(ConsumeContext<AuthPasswordHasBeenChanged> context)
        => HandleAsync(context.Message);
}
