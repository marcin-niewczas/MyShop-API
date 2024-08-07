﻿using Microsoft.AspNetCore.SignalR;
using MyShop.Application.Hubs.Shared;
using MyShop.Application.Hubs.Shared.Interfaces;
using MyShop.Application.Mappings;
using MyShop.Core.Models.Notifications;

namespace MyShop.Infrastructure.Notifications.Commons;
internal sealed class HubCommonNotification(
    IHubContext<NotificationsHub, INotificationsHub> notificationHub
    ) : ICommonNotification
{
    public NotificationSenderType NotificationSenderType => NotificationSenderType.Hub;

    public Task NotifyAsync(Guid registeredUserId, Notification notification, CancellationToken cancellationToken = default)
         => notificationHub.Clients.Group(registeredUserId.ToString()).ReceiveNotification(notification.ToNotificationDto());
}
