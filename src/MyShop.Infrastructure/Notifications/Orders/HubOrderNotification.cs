using Microsoft.AspNetCore.SignalR;
using MyShop.Application.Hubs.Shared;
using MyShop.Application.Hubs.Shared.Interfaces;
using MyShop.Application.Mappings;
using MyShop.Core.Models.Notifications;
using MyShop.Core.Models.Orders;

namespace MyShop.Infrastructure.Notifications.Orders;
internal sealed class HubOrderNotification(
    IHubContext<NotificationsHub, INotificationsHub> notificationHub
    ) : IOrderNotfication
{
    public NotificationSenderType NotificationSenderType => NotificationSenderType.Hub;

    public Task NotifyAsync(
        Order order,
        OrderNotificationType orderNotificationType,
        Notification? notification,
        CancellationToken cancellationToken = default
        ) => (notification is not null) switch
        {
            true => notificationHub.Clients.Group(order.UserId.ToString()).ReceiveNotification(notification.ToNotificationDto()),
            _ => Task.CompletedTask
        };
}
