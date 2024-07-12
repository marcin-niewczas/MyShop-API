using MyShop.Application.Responses;
using MyShop.Core.Dtos.Auth;

namespace MyShop.Application.Commands.Account.Users;
public sealed record RemoveUserPhotoAc : ICommand<ApiResponse<UserDto>>;
