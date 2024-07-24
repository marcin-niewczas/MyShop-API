using MyShop.Core.Models.Notifications;
using MyShop.Infrastructure.Notifications.Commons;
using MyShop.Infrastructure.Notifications.Senders.Interfaces;

namespace MyShop.Infrastructure.Notifications.Senders;
internal sealed class CommonNotificationsSender(
    IEnumerable<ICommonNotification> commonNotfications
    ) : ICommonNotificationsSender
{
    public Task SendAsync(
        Guid registeredUserId,
        Notification notification,
        NotificationSenderType[]? chosenNotificationSenderTypes = null,
        CancellationToken cancellationToken = default
        ) => Task.WhenAll(chosenNotificationSenderTypes switch
        {
            null => commonNotfications
                .Select(n => n.NotifyAsync(registeredUserId, notification, cancellationToken)),
            _ => commonNotfications
                .Where(n => chosenNotificationSenderTypes.Contains(n.NotificationSenderType))
                .Select(n => n.NotifyAsync(registeredUserId, notification, cancellationToken))
        });
}
