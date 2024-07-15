using MyShop.Core.Models.Notifications;
using MyShop.Core.Models.Orders;
using MyShop.Core.Models.Users;
using MyShop.Infrastructure.Notifications.Orders;
using MyShop.Infrastructure.Notifications.Senders.Interfaces;

namespace MyShop.Infrastructure.Notifications.Senders;
internal sealed class OrderNotificationsSender(
    IEnumerable<IOrderNotfication> orderNotfications
    ) : IOrderNotificationsSender
{
    public Task SendAsync(
        Order order,
        OrderNotificationType orderNotificationType,
        Notification? notification,
        NotificationSenderType[]? chosenNotificationSenderTypes = null,
        CancellationToken cancellationToken = default
        )
    {
        ArgumentNullException.ThrowIfNull(order, nameof(order));
        ArgumentNullException.ThrowIfNull(order.User, nameof(order));

        return Task.WhenAll(chosenNotificationSenderTypes switch
        {
            null => orderNotfications
                .Where(n => HasPermission(n.NotificationSenderType, order.User))
                .Select(n => n.NotifyAsync(order, orderNotificationType, notification, cancellationToken)),
            _ => orderNotfications
                .Where(n => chosenNotificationSenderTypes.Contains(n.NotificationSenderType) && HasPermission(n.NotificationSenderType, order.User))
                .Select(n => n.NotifyAsync(order, orderNotificationType, notification, cancellationToken))
        });
    }


    private static bool HasPermission(NotificationSenderType notificationSenderType, User user)
        => user switch
        {
            RegisteredUser => true,
            User => notificationSenderType == NotificationSenderType.Email,
        };

}
