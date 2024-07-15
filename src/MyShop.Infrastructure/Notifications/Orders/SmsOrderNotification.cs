using Microsoft.Extensions.Logging;
using MyShop.Core.Models.Notifications;
using MyShop.Core.Models.Orders;

namespace MyShop.Infrastructure.Notifications.Orders;
internal sealed class SmsOrderNotification(
    ILogger<SmsOrderNotification> logger
    ) : IOrderNotfication
{
    public NotificationSenderType NotificationSenderType => NotificationSenderType.Sms;

    public Task NotifyAsync(
        Order order,
        OrderNotificationType orderNotificationType,
        Notification? notification,
        CancellationToken cancellationToken = default
        )
    {
        logger.LogInformation("Sent {NotificationSenderType} to {PhoneNumber}.", NotificationSenderType, order.PhoneNumber);

        return Task.CompletedTask;
    }
}
