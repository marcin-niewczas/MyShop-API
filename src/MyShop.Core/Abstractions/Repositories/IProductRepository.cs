using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;

namespace MyShop.Core.Abstractions.Repositories;
public interface IProductRepository : IBaseReadRepository<Product>, IBaseWriteRepository<Product>
{
    Task<Product?> GetProductByIdForCreateProductVariantAsync(
        Guid id,
        CancellationToken cancellationToken = default
        );
    Task<IReadOnlyCollection<string>> GetProductNamesAsync(
        string searchPhrase,
        int take,
        CancellationToken cancellationToken = default
        );
    Task<PagedResult<PagedProductMpDto>> GetPagedProductsMpAsync(
        int pageNumber,
        int pageSize,
        GetPagedProductsMpSortBy? sortBy,
        SortDirection? sortDirection,
        string? searchPhrase,
        CancellationToken cancellationToken = default
        );
    Task<ProductFiltersEc> GetProductFiltersEcAsync(
        IEnumerable<Guid> categoryIds,
        decimal? minPrice,
        decimal? maxPrice,
        IReadOnlyDictionary<string, string[]>? productOptionParams,
        CancellationToken cancellationToken = default
        );
    Task<ProductMpDto?> GetProductMpAsync(
        Guid id,
        CancellationToken cancellationToken = default
        );
}
