using MyShop.Application.Dtos.Account.Users;
using MyShop.Application.Responses;

namespace MyShop.Application.Queries.Account.Users;
public sealed record GetUserAddressAc(
    Guid Id
    ) : IQuery<ApiResponse<UserAddressAcDto>>;
