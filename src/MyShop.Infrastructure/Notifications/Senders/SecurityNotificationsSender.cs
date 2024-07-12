using MyShop.Core.Models.Notifications;
using MyShop.Infrastructure.Notifications.Securities;
using MyShop.Infrastructure.Notifications.Senders.Interfaces;

namespace MyShop.Infrastructure.Notifications.Senders;
internal sealed class SecurityNotificationsSender(
    IEnumerable<ISecurityNotification> securityNotfications
    ) : ISecurityNotificationsSender
{
    public Task SendAsync(
        Guid registeredUserId,
        NotificationRegisteredUser notification,
        NotificationSenderType[]? chosenNotificationSenderTypes = null,
        CancellationToken cancellationToken = default
        ) => Task.WhenAll(chosenNotificationSenderTypes switch
        {
            null => securityNotfications
                .Select(n => n.NotifyAsync(registeredUserId, notification, cancellationToken)),
            _ => securityNotfications
                .Where(n => chosenNotificationSenderTypes.Contains(n.NotificationSenderType))
                .Select(n => n.NotifyAsync(registeredUserId, notification, cancellationToken))
        });
}
