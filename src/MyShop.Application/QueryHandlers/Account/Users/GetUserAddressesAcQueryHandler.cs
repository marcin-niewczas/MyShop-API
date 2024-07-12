using MyShop.Application.Abstractions;
using MyShop.Application.Dtos.Account.Users;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.Account.Users;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.RepositoryQueryParams.Commons;

namespace MyShop.Application.QueryHandlers.Account.Users;
internal sealed class GetUserAddressesAcQueryHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetUserAddressesAc, ApiResponseWithCollection<UserAddressAcDto>>
{
    public async Task<ApiResponseWithCollection<UserAddressAcDto>> HandleAsync(
        GetUserAddressesAc query,
        CancellationToken cancellationToken = default
        )
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var addresses = await unitOfWork.UserAddressRepository.GetByPredicateAsync(
            predicate: p => p.RegisteredUserId == userId,
            sortByKeySelector: o => o.IsDefault,
            sortDirection: SortDirection.Descendant,
            thenSortByKeySelector: o => o.UpdatedAt == null ? o.CreatedAt : o.UpdatedAt,
            thenSortDirection: SortDirection.Descendant,
            cancellationToken: cancellationToken
            );

        return new(addresses.ToUserAddressAcDtos());
    }
}
