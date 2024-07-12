using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Photos;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class ProductVariantPhotoRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<ProductVariantPhoto>(dbContext),
        IProductVariantPhotoRepository
{
}
