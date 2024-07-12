using MyShop.Core.Models.Products;

namespace MyShop.Core.Abstractions.Repositories;
public interface IProductDetailOptionRepository : IBaseReadRepository<ProductDetailOption>, IBaseWriteRepository<ProductDetailOption>
{
}
