using MyShop.Core.Models.Notifications;
using MyShop.Core.Models.Orders;
using MyShop.Infrastructure.Notifications.Orders;

namespace MyShop.Infrastructure.Notifications.Senders.Interfaces;
public interface IOrderNotificationsSender
{
    Task SendAsync(
        Order order,
        OrderNotificationType orderNotificationType,
        NotificationRegisteredUser? notification,
        NotificationSenderType[]? chosenNotificationSenderTypes = null,
        CancellationToken cancellationToken = default
        );
}
