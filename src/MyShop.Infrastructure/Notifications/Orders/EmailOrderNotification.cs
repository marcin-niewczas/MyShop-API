using Microsoft.Extensions.Logging;
using MyShop.Core.Models.Notifications;
using MyShop.Core.Models.Orders;

namespace MyShop.Infrastructure.Notifications.Orders;
internal class EmailOrderNotification(
    ILogger<EmailOrderNotification> logger
    ) : IOrderNotfication
{
    public NotificationSenderType NotificationSenderType => NotificationSenderType.Email;

    public Task NotifyAsync(
        Order order,
        OrderNotificationType orderNotificationType,
        NotificationRegisteredUser? notification,
        CancellationToken cancellationToken = default
        )
    {
        logger.LogInformation("Sent {NotificationSenderType} to {Email}.", NotificationSenderType, order.Email);

        return Task.CompletedTask;
    }
}
