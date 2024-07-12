using MyShop.Core.HelperModels;
using MyShop.Core.Models.ShoppingCarts;

namespace MyShop.Core.Abstractions.Repositories;
public interface IShoppingCartRepository : IBaseReadRepository<ShoppingCart>, IBaseWriteRepository<ShoppingCart>
{
    Task<ShoppingCart?> GetUserShoppingCartWithItemsAndProductVariantsAsync(
        Guid userId,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<ShoppingCartVerifier?> GetShoppingCartVerifierAsync(
        Guid userId,
        CancellationToken cancellationToken = default
        );
}
