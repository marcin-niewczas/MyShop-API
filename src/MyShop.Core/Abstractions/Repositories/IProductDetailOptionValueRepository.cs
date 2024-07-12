using MyShop.Core.Models.Products;

namespace MyShop.Core.Abstractions.Repositories;
public interface IProductDetailOptionValueRepository : IBaseReadRepository<ProductDetailOptionValue>, IBaseWriteRepository<ProductDetailOptionValue>
{
}
