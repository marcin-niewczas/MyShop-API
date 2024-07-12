using MyShop.Core.Models.Users;

namespace MyShop.Core.Abstractions.Repositories;
public interface IUserRepository : IBaseReadRepository<User>, IBaseWriteRepository<User>
{
    Task<User?> GetUserWithShoppingCartItemsAndProductVariantsAsync(
        Guid userId,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );
}
