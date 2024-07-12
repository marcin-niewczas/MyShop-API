using MyShop.Application.Dtos.Shared.Notifications;

namespace MyShop.Application.Responses.ExtensionResponses;
public sealed record GetNotificationsApiPagedResponse : ApiPagedResponse<NotificationDto>
{
    public int? UnreadNotificationCount { get; }

    public GetNotificationsApiPagedResponse(
        IReadOnlyCollection<NotificationDto> dtos,
        int totalCount,
        int pageNumber,
        int pageSize,
        int? unreadNotificationCount
        ) : base(
                dtos,
                totalCount,
                pageNumber,
                pageSize
            )
    {
        UnreadNotificationCount = unreadNotificationCount;
    }
}
