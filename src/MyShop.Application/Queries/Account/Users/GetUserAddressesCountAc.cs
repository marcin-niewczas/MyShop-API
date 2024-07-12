using MyShop.Application.Dtos;
using MyShop.Application.Responses;

namespace MyShop.Application.Queries.Account.Users;
public sealed record GetUserAddressesCountAc
    : IQuery<ApiResponse<ValueDto<int>>>;
