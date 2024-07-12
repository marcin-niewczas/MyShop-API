using Microsoft.EntityFrameworkCore;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Products;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class ProductVariantOptionValueRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<ProductVariantOptionValue>(dbContext), IProductVariantOptionValueRepository
{

    public async Task<IReadOnlyCollection<ProductVariantOptionValue>> GetSortedProductVariantOptionValuesAsync(
        Guid productId,
        IReadOnlyCollection<Guid> productVariantOptionValueIds,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        )
    {
        var baseQuery = _dbContext
            .ProductProductVariantOptions
            .Where(o => o.ProductId == productId)
            .OrderBy(e => e.Position)
            .Join(_dbSet.Where(v => productVariantOptionValueIds.Contains(v.Id)),
                    k => k.ProductVariantOptionId,
                    k => k.ProductOptionId, (_, v) => v);

        if (!withTracking)
        {
            baseQuery = baseQuery.AsNoTracking();
        }

        return await baseQuery.ToArrayAsync(cancellationToken);
    }
}
