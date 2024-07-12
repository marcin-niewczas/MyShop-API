using MyShop.Core.Models.Products;

namespace MyShop.Core.Abstractions.Repositories;
public interface IProductProductDetailOptionValueRepository
    : IBaseReadRepository<ProductProductDetailOptionValue>,
      IBaseWriteRepository<ProductProductDetailOptionValue>
{
}
