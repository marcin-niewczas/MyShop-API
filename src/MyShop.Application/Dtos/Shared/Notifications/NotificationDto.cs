using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.Shared.Notifications;
public sealed record NotificationDto : BaseTimestampDto
{
    public required bool IsRead { get; init; }
    public required string NotificationType { get; init; }
    public required string Message { get; init; }
    public required string? ResourceId { get; init; }
}
