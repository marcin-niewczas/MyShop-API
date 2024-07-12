using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Products;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class ProductProductVariantOptionRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<ProductProductVariantOption>(dbContext), 
        IProductProductVariantOptionRepository
{
}
