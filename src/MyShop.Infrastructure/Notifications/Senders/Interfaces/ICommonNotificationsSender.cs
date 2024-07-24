using MyShop.Core.Models.Notifications;

namespace MyShop.Infrastructure.Notifications.Senders.Interfaces;
internal interface ICommonNotificationsSender
{
    Task SendAsync(
        Guid registeredUserId,
        Notification notification,
        NotificationSenderType[]? chosenNotificationSenderTypes = null,
        CancellationToken cancellationToken = default
        );
}
