using MyShop.Application.Responses;
using MyShop.Core.Dtos.Auth;

namespace MyShop.Application.Queries.Auth;
public record class GetUserMeAuth : IQuery<ApiResponse<UserDto>>;
