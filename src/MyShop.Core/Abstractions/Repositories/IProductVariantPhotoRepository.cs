using MyShop.Core.Models.Photos;

namespace MyShop.Core.Abstractions.Repositories;
public interface IProductVariantPhotoRepository : IBaseReadRepository<ProductVariantPhoto>, IBaseWriteRepository<ProductVariantPhoto>
{
}
