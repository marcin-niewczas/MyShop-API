using MyShop.Application.Dtos.Auth;
using MyShop.Application.Responses;

namespace MyShop.Application.Commands.Auth;
public sealed record RefreshAccessTokenAuth(
    string RefreshToken,
    Guid UserTokenId
    ) : ICommand<ApiResponse<AuthDto>>;
