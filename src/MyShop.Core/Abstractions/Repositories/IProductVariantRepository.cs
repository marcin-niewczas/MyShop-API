using MyShop.Core.Dtos.ECommerce;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Dtos.Shared;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;
using MyShop.Core.RepositoryQueryParams.ECommerce;

namespace MyShop.Core.Abstractions.Repositories;
public interface IProductVariantRepository : IBaseReadRepository<ProductVariant>, IBaseWriteRepository<ProductVariant>
{
    Task<ProductVariantMpDto?> GetProductVariantMpAsync(
        Guid id,
        CancellationToken cancellationToken = default
        );
    Task<PagedResult<ProductVariant>> GetPagedProductVariantsMpByProductIdAsync(
        Guid productId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default
        );
    Task<IReadOnlyCollection<ProductVariantEcDto>> GetProductVariantsByEncodedNameAsync(
        string encodedName,
        CancellationToken cancellationToken = default
        );
    Task<BaseProductDetailEcDto?> GetProductDetailAsync(
        string encodedName,
        CancellationToken cancellationToken = default
        );
    Task<PagedResult<ProductItemDto>> GetPagedDataByCategoryIdsAsync(
       int pageNumber,
       int pageSize,
       GetPagedProductsEcSortBy sortBy,
       IReadOnlyCollection<Guid>? categoryIds,
       IReadOnlyDictionary<string, string[]>? productOptionParam,
       decimal? minPrice,
       decimal? maxPrice,
       string? searchPhrase,
       CancellationToken cancellationToken = default
       );
}
