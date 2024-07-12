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
    ISecurityNotificationsSender securityNotificationsSender
    ) : IEventHandler<AuthEmailHasBeenChanged>
{
    public async Task HandleAsync(AuthEmailHasBeenChanged @event, CancellationToken cancellationToken = default)
    {
        var notification = new NotificationRegisteredUser(
            @event.RegisteredUserId,
            NotificationType.Security,
            $"Your {nameof(RegisteredUser.Email)} has been changed."
            );

        await unitOfWork.AddAsync(notification, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await securityNotificationsSender.SendAsync(
            @event.RegisteredUserId,
            notification,
            cancellationToken: cancellationToken
            );
    }
}
