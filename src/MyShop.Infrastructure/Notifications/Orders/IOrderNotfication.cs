using MyShop.Core.Models.Notifications;
using MyShop.Core.Models.Orders;

namespace MyShop.Infrastructure.Notifications.Orders;
internal interface IOrderNotfication
{
    NotificationSenderType NotificationSenderType { get; }
    Task NotifyAsync(
        Order order,
        OrderNotificationType orderNotificationType,
        NotificationRegisteredUser? notification,
        CancellationToken cancellationToken = default
        );
}
