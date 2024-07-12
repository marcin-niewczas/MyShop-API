using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;
using MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories.Utils;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class BaseProductOptionValueRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<BaseProductOptionValue>(dbContext), 
        IBaseProductOptionValueRepository
{
    public Task<PagedResult<BaseProductOptionValue>> GetPagedDataByProductOptionIdAsync(
       Guid productOptionId,
       int pageNumber,
       int pageSize,
       string? searchPhrase,
       CancellationToken cancellationToken = default
       ) => _dbSet
               .Where(e => e.ProductOptionId == productOptionId && (searchPhrase == null || e.Value.ToLower().Contains(searchPhrase.ToLower())))
               .OrderBy(e => e.Position)
               .ToPagedResultAsync(pageNumber, pageSize, cancellationToken: cancellationToken);
}
