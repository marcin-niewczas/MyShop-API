using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.Notifications;

namespace MyShop.Core.Models.Notifications;
public sealed class Notification : BaseTimestampEntity
{
    public NotificationType NotificationType { get; private set; } = default!;
    public string Message { get; private set; } = default!;
    public string? ResourceId { get; private set; }
    public IReadOnlyCollection<RegisteredUser> RegisteredUsers { get; private set; } = default!;
    public IReadOnlyCollection<NotificationRegisteredUser> NotificationRegisteredUsers { get; private set; } = default!;

    private Notification() { }

    private Notification(
        NotificationType notificationType,
        string message,
        string? resourceId = null
        )
    {
        ArgumentNullException.ThrowIfNull(nameof(notificationType));
        ArgumentException.ThrowIfNullOrWhiteSpace(nameof(message));

        NotificationType = notificationType;
        Message = message;
        ResourceId = resourceId;
    }

    public Notification(
        NotificationType notificationType,
        string message,
        Guid registeredUserId,
        string? resourceId = null
        ) : this(
            notificationType,
            message,
            resourceId
            )
    {
        NotificationRegisteredUsers = [new(Id, registeredUserId)];
    }

    public Notification(
        NotificationType notificationType,
        string message,
        IEnumerable<Guid> registeredUserIds,
        string? resourceId = null
        ) : this(
            notificationType,
            message,
            resourceId
            )
    {
        ArgumentNullException.ThrowIfNull(nameof(registeredUserIds));

        NotificationRegisteredUsers = registeredUserIds
            .Select(id => new NotificationRegisteredUser(Id, id))
            .ToList();
    }
}
