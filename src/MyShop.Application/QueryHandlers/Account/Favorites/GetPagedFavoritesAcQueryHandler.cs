using MyShop.Application.Abstractions;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.Account.Favorites;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.Shared;
using MyShop.Core.RepositoryQueryParams.Account;

namespace MyShop.Application.QueryHandlers.Account.Favorites;
internal sealed class GetPagedFavoritesAcQueryHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedFavoritesAc, ApiPagedResponse<ProductItemDto>>
{
    public async Task<ApiPagedResponse<ProductItemDto>> HandleAsync(
        GetPagedFavoritesAc query,
        CancellationToken cancellationToken = default
        )
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var pagedResult = await unitOfWork.FavoriteRepository.GetPagedFavoritesProductItemsAsync(
            userId,
            query.PageNumber,
            query.PageSize,
            TypeMapper.MapOptionalEnum<GetPagedFavoritesAcSortBy>(query.SortBy),
            TypeMapper.MapOptionalSortDirection(query.SortDirection),
            query.SearchPhrase,
            cancellationToken
            );

        return new(
            dtos: pagedResult.Data,
            totalCount: pagedResult.TotalCount,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize
            );
    }
}
