using MyShop.Core.Models.ShoppingCarts;

namespace MyShop.Core.Abstractions.Repositories;
public interface IShoppingCartItemRepository
    : IBaseReadRepository<ShoppingCartItem>, IBaseWriteRepository<ShoppingCartItem>
{
}
