using MyShop.Application.Abstractions;
using MyShop.Application.Dtos;
using MyShop.Application.Queries.Account.Users;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;

namespace MyShop.Application.QueryHandlers.Account.Users;
internal sealed class GetUserAddressesCountAcQueryHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetUserAddressesCountAc, ApiResponse<ValueDto<int>>>
{
    public async Task<ApiResponse<ValueDto<int>>> HandleAsync(
        GetUserAddressesCountAc query, 
        CancellationToken cancellationToken = default
        )
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var count = await unitOfWork.UserAddressRepository.CountAsync(
            predicate: e => e.RegisteredUserId.Equals(userId),
            cancellationToken: cancellationToken
            );

        return new(new(count));
    }
}
