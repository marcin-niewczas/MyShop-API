using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;

namespace MyShop.Core.Abstractions.Repositories;
public interface IBaseProductOptionValueRepository : IBaseReadRepository<BaseProductOptionValue>, IBaseWriteRepository<BaseProductOptionValue>
{
    Task<PagedResult<BaseProductOptionValue>> GetPagedDataByProductOptionIdAsync(
       Guid productOptionId,
       int pageNumber,
       int pageSize,
       string? searchPhrase,
       CancellationToken cancellationToken = default
       );
}
