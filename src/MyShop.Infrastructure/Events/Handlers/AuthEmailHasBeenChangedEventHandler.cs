using MyShop.Application.Abstractions;
using MyShop.Application.Events;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Notifications;
using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.Notifications;
using MyShop.Infrastructure.Notifications.Senders.Interfaces;

namespace MyShop.Infrastructure.Events.Handlers;
internal sealed class AuthEmailHasBeenChangedEventHandler(
    IUnitOfWork unitOfWork,
    ICommonNotificationsSender commonNotificationsSender
    ) : IEventHandler<AuthEmailHasBeenChanged>
{
    public async Task HandleAsync(AuthEmailHasBeenChanged @event, CancellationToken cancellationToken = default)
    {
        var notification = new Notification(
            NotificationType.Security,
            $"Your {nameof(RegisteredUser.Email)} has been changed.",
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
}
