using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;
using MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories.Utils;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class ProductVariantOptionRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<ProductVariantOption>(dbContext), IProductVariantOptionRepository
{
    public Task<PagedResult<ProductVariantOption>> GetPagedDataByProductIdAsync(
        Guid productId,
        int pageNumber,
        int pageSize,
        string? searchPhrase,
        CancellationToken cancellationToken = default
        )
    {
        var baseQuery = _dbContext.ProductProductVariantOptions
            .Where(e => e.Product.Id == productId && (searchPhrase == null || e.Product.Name.ToLower().Contains(searchPhrase.ToLower())))
            .OrderBy(e => e.Position)
            .Select(e => e.ProductVariantOption);

        return baseQuery.ToPagedResultAsync(
            pageNumber,
            pageSize,
            cancellationToken: cancellationToken
            );
    }
}
