using MyShop.Application.Dtos.Shared.Notifications;
using MyShop.Core.Models.Notifications;

namespace MyShop.Application.Mappings;
public static class NotificationMappingExtension
{
    public static NotificationDto ToNotificationDto(this NotificationRegisteredUser entity)
        => new()
        {
            Id = entity.Notification.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            NotificationType = entity.Notification.NotificationType,
            IsRead = entity.IsRead,
            Message = entity.Notification.Message,
            ResourceId = entity.Notification.ResourceId,
        };

    public static NotificationDto ToNotificationDto(this Notification entity)
        => new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            NotificationType = entity.NotificationType,
            IsRead = false,
            Message = entity.Message,
            ResourceId = entity.ResourceId,
        };

    public static IReadOnlyCollection<NotificationDto> ToNotificationDtos(this IEnumerable<NotificationRegisteredUser> entities)
        => entities.Select(ToNotificationDto).ToArray();
}
