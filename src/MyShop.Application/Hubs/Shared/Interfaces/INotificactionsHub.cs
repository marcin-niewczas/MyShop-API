using MyShop.Application.Dtos.Shared.Notifications;

namespace MyShop.Application.Hubs.Shared.Interfaces;
public interface INotificationsHub
{
    Task ReceiveNotification(NotificationDto dto);
}
