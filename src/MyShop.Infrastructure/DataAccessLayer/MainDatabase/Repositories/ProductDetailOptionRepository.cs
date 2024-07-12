using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Products;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class ProductDetailOptionRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<ProductDetailOption>(dbContext), 
        IProductDetailOptionRepository
{
}
