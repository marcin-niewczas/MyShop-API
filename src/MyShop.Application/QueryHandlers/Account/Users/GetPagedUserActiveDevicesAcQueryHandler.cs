using MyShop.Application.Abstractions;
using MyShop.Application.Dtos.Account.Users;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.Account.Users;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.RepositoryQueryParams.Commons;

namespace MyShop.Application.QueryHandlers.Account.Users;
internal sealed class GetPagedUserActiveDevicesAcQueryHandler(
    IUnitOfWork unitOfWork,
    IUserClaimsService userClaimsService
    ) : IQueryHandler<GetPagedUserActiveDevicesAc, ApiPagedResponse<UserActiveDeviceAcDto>>
{
    public async Task<ApiPagedResponse<UserActiveDeviceAcDto>> HandleAsync(GetPagedUserActiveDevicesAc query, CancellationToken cancellationToken = default)
    {
        var userClaimsData = userClaimsService.GetUserClaimsData();

        var pagedResult = await unitOfWork.UserTokenRepository.GetPagedDataAsync(
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            predicate: p => p.UserId == userClaimsData.UserId,
            sortByKeySelector: o => o.Id == userClaimsData.UserTokenId,
            sortDirection: SortDirection.Descendant,
            thenSortByKeySelector: o => o.UpdatedAt == null ? o.CreatedAt : o.UpdatedAt,
            thenSortDirection: SortDirection.Descendant,
            cancellationToken: cancellationToken
            );

        return new(
            dtos: pagedResult.Data.ToUserActiveDeviceAcDtos(userClaimsData.UserTokenId),
            totalCount: pagedResult.TotalCount,
            pageSize: query.PageSize,
            pageNumber: query.PageNumber
            );
    }
}
