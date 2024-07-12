using MyShop.Application.Dtos.Auth;
using MyShop.Application.Responses;

namespace MyShop.Application.Commands.Auth;
public sealed record SignUpGuestAuth : ICommand<ApiResponse<AuthDto>>;
