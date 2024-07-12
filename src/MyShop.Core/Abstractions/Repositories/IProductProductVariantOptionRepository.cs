using MyShop.Core.Models.Products;

namespace MyShop.Core.Abstractions.Repositories;
public interface IProductProductVariantOptionRepository
    : IBaseReadRepository<ProductProductVariantOption>,
      IBaseWriteRepository<ProductProductVariantOption>
{
}
