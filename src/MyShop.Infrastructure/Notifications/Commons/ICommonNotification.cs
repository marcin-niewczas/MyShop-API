using MyShop.Core.Models.Notifications;

namespace MyShop.Infrastructure.Notifications.Commons;
internal interface ICommonNotification
{
    NotificationSenderType NotificationSenderType { get; }
    Task NotifyAsync(
        Guid registeredUserId,
        Notification notification,
        CancellationToken cancellationToken = default
        );
}
