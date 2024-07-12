using MyShop.Core.Models.Products;

namespace MyShop.Core.Abstractions.Repositories;
public interface IProductVariantOptionValueRepository : IBaseReadRepository<ProductVariantOptionValue>, IBaseWriteRepository<ProductVariantOptionValue>
{
    Task<IReadOnlyCollection<ProductVariantOptionValue>> GetSortedProductVariantOptionValuesAsync(
        Guid productId,
        IReadOnlyCollection<Guid> productVariantOptionValueIds,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );
}
