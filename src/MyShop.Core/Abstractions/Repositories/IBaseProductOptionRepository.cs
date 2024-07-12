using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;

namespace MyShop.Core.Abstractions.Repositories;
public interface IBaseProductOptionRepository : IBaseReadRepository<BaseProductOption>, IBaseWriteRepository<BaseProductOption>
{
    Task<PagedResult<BaseProductOption>> GetPagedDataAsync(
        int pageNumber,
        int pageSize,
        ProductOptionTypeMpQueryType productOptionTypeQueryType,
        ProductOptionSubtypeMpQueryType productOptionSubtypeQueryType,
        GetPagedProductOptionsMpSortBy? sortBy,
        SortDirection? sortDirection,
        string? searchPhrase,
        CancellationToken cancellationToken = default
        );
}
