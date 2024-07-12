using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.Notifications;

namespace MyShop.Core.Models.Notifications;
public sealed class NotificationRegisteredUser : BaseTimestampEntity
{
    public Notification Notification { get; private set; } = default!;
    public Guid NotificationId { get; private set; }
    public RegisteredUser RegisteredUser { get; private set; } = default!;
    public Guid RegisteredUserId { get; private set; }
    public bool IsRead { get; private set; }

    private NotificationRegisteredUser() { }

    public NotificationRegisteredUser(
        Guid registeredUserId,
        Guid notificationId
        )
    {
        NotificationId = notificationId;
        RegisteredUserId = registeredUserId;
    }

    public NotificationRegisteredUser(
        Guid registeredUserId,
        NotificationType notificationType,
        string message,
        string? resourceId = null
        )
    {
        Notification = new(notificationType, message, registeredUserId, resourceId);
        NotificationId = Notification.Id;
        RegisteredUserId = registeredUserId;
    }

    public void SetAsRead()
    {
        IsRead = true;
    }
}
