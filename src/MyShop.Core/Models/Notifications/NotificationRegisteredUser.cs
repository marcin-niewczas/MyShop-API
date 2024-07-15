using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Users;

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
        Guid notificationId,
        Guid registeredUserId
        )
    {
        NotificationId = notificationId;
        RegisteredUserId = registeredUserId;
    }

    public void SetAsRead()
    {
        IsRead = true;
    }
}
