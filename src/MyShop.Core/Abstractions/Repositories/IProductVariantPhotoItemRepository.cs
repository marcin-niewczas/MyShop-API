using MyShop.Core.Models.Products;

namespace MyShop.Core.Abstractions.Repositories;
public interface IProductVariantPhotoItemRepository 
    : IBaseReadRepository<ProductVariantPhotoItem>, 
      IBaseWriteRepository<ProductVariantPhotoItem>
{
}
