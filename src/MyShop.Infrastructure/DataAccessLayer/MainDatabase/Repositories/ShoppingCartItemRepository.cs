using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.ShoppingCarts;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal class ShoppingCartItemRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<ShoppingCartItem>(dbContext),
        IShoppingCartItemRepository
{
}
