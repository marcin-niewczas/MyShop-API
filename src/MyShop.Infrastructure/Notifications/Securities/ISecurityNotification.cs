using MyShop.Core.Models.Notifications;

namespace MyShop.Infrastructure.Notifications.Securities;
internal interface ISecurityNotification
{
    NotificationSenderType NotificationSenderType { get; }
    Task NotifyAsync(
        Guid registeredUserId,
        NotificationRegisteredUser notification,
        CancellationToken cancellationToken = default
        );
}
