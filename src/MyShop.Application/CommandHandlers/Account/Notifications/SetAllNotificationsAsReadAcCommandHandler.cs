using MyShop.Application.Abstractions;
using MyShop.Application.Commands.Account.Notifications;
using MyShop.Application.Dtos;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;

namespace MyShop.Application.CommandHandlers.Account.Notifications;
internal sealed class SetAllNotificationsAsReadAcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<SetAllNotificationsAsRead, ApiResponse<ValueDto<int>>>
{
    public async Task<ApiResponse<ValueDto<int>>> HandleAsync(SetAllNotificationsAsRead command, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        await unitOfWork.NotificationRegisteredUserRepository.ExecuteUpdateAsync(
            predicate: e => e.RegisteredUserId == userId && !e.IsRead,
            setPropertyCalls: setter => setter.SetProperty(e => e.IsRead, true),
            cancellationToken: cancellationToken
            );

        var unreadNotificationCount = await unitOfWork.NotificationRegisteredUserRepository.CountAsync(
                predicate: e => e.RegisteredUserId == userId && !e.IsRead,
                cancellationToken: cancellationToken
                );

        return new(new(unreadNotificationCount));
    }
}
