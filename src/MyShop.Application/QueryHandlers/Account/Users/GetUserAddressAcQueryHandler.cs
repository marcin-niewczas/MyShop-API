using MyShop.Application.Abstractions;
using MyShop.Application.Dtos.Account.Users;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.Account.Users;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Users;
using MyShop.Core.Utils;

namespace MyShop.Application.QueryHandlers.Account.Users;
internal sealed class GetUserAddressAcQueryHandler(
    IUnitOfWork unitOfWork,
    IUserClaimsService userClaimsService
    ) : IQueryHandler<GetUserAddressAc, ApiResponse<UserAddressAcDto>>
{
    public async Task<ApiResponse<UserAddressAcDto>> HandleAsync(GetUserAddressAc query, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var entity = await unitOfWork.UserAddressRepository.GetFirstByPredicateAsync(
            predicate: e => e.RegisteredUserId == userId && e.Id == query.Id,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException($"Not found {nameof(UserAddress).ToTitleCase()}.");

        return new(entity.ToUserAddressAcDto());
    }
}
