using MyShop.Application.Abstractions;
using MyShop.Application.Commands.Account.Notifications;
using MyShop.Application.Dtos;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Notifications;

namespace MyShop.Application.CommandHandlers.Account.Notifications;
internal sealed class SetNotificationAsReadAcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<SetNotificationAsRead, ApiResponse<ValueDto<int>>>
{
    public async Task<ApiResponse<ValueDto<int>>> HandleAsync(SetNotificationAsRead command, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var entity = await unitOfWork.NotificationRegisteredUserRepository.GetFirstByPredicateAsync(
            predicate: e => e.NotificationId == command.Id && e.RegisteredUserId == userId,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(Notification), command.Id);

        entity.SetAsRead();

        await unitOfWork.UpdateAsync(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var unreadNotificationCount = await unitOfWork.NotificationRegisteredUserRepository.CountAsync(
            predicate: e => e.RegisteredUserId == userId && !e.IsRead,
            cancellationToken: cancellationToken
            );

        return new(new(unreadNotificationCount));
    }
}
