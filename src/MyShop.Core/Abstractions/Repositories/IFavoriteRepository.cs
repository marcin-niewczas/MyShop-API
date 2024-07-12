using MyShop.Core.Dtos.Shared;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;
using MyShop.Core.RepositoryQueryParams.Account;
using MyShop.Core.RepositoryQueryParams.Commons;

namespace MyShop.Core.Abstractions.Repositories;
public interface IFavoriteRepository : IBaseReadRepository<Favorite>, IBaseWriteRepository<Favorite>
{
    Task<PagedResult<ProductItemDto>> GetPagedFavoritesProductItemsAsync(
        Guid userId,
        int pageNumber,
        int pageSize,
        GetPagedFavoritesAcSortBy? sortBy,
        SortDirection? sortDirection,
        string? searchPhrase = default,
        CancellationToken cancellationToken = default
        );
}
