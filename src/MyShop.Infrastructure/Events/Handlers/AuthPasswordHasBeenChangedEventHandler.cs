using MyShop.Application.Abstractions;
using MyShop.Application.Events;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Notifications;
using MyShop.Core.ValueObjects.Notifications;
using MyShop.Infrastructure.Notifications.Senders.Interfaces;

namespace MyShop.Infrastructure.Events.Handlers;
internal sealed class AuthPasswordHasBeenChangedEventHandler(
    IUnitOfWork unitOfWork,
    ISecurityNotificationsSender securityNotificationsSender
    ) : IEventHandler<AuthPasswordHasBeenChanged>
{
    public async Task HandleAsync(AuthPasswordHasBeenChanged @event, CancellationToken cancellationToken = default)
    {
        var notification = new NotificationRegisteredUser(
            @event.RegisteredUserId,
            NotificationType.Security,
            "Your Password has been changed."
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
