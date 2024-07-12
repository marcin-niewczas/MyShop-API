using MyShop.Core.Models.Notifications;

namespace MyShop.Infrastructure.Notifications.Senders.Interfaces;
internal interface ISecurityNotificationsSender
{
    Task SendAsync(
        Guid registeredUserId,
        NotificationRegisteredUser notification,
        NotificationSenderType[]? chosenNotificationSenderTypes = null,
        CancellationToken cancellationToken = default
        );
}
