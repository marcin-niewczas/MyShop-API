using Microsoft.AspNetCore.SignalR;
using MyShop.Application.Hubs.Shared;
using MyShop.Application.Hubs.Shared.Interfaces;
using MyShop.Application.Mappings;
using MyShop.Core.Models.Notifications;

namespace MyShop.Infrastructure.Notifications.Securities;
internal sealed class HubSecurityNotification(
    IHubContext<NotificationsHub, INotificationsHub> notificationHub
    ) : ISecurityNotification
{
    public NotificationSenderType NotificationSenderType => NotificationSenderType.Hub;

    public Task NotifyAsync(Guid registeredUserId, NotificationRegisteredUser notification, CancellationToken cancellationToken = default)
         => notificationHub.Clients.Group(registeredUserId.ToString()).ReceiveNotification(notification.ToNotificationDto());
}
