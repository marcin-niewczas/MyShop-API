using MyShop.Application.Dtos;
using MyShop.Application.Responses;

namespace MyShop.Application.Commands.Account.Notifications;
public sealed record SetNotificationAsRead(
    Guid Id
    ) : ICommand<ApiResponse<ValueDto<int>>>;
