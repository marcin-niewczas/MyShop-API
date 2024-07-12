using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Products;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class ProductDetailOptionValueRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<ProductDetailOptionValue>(dbContext), 
        IProductDetailOptionValueRepository
{
}
