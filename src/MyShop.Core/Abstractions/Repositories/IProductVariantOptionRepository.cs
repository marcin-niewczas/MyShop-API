using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;

namespace MyShop.Core.Abstractions.Repositories;
public interface IProductVariantOptionRepository : IBaseReadRepository<ProductVariantOption>, IBaseWriteRepository<ProductVariantOption>
{
    Task<PagedResult<ProductVariantOption>> GetPagedDataByProductIdAsync(
        Guid productId,
        int pageNumber,
        int pageSize,
        string? searchPhrase,
        CancellationToken cancellationToken = default
        );
}
