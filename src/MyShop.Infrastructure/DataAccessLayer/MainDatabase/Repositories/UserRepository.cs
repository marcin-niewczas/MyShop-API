using Microsoft.EntityFrameworkCore;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Users;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class UserRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<User>(dbContext),
        IUserRepository
{
    public Task<User?> GetUserWithShoppingCartItemsAndProductVariantsAsync(
        Guid userId,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        )
    {
        var baseQuery = _dbSet
            .Include(i => i.ShoppingCart)
            .ThenInclude(i => i.ShoppingCartItems)
            .ThenInclude(i => i.ProductVariant)
            .AsQueryable();

        baseQuery = withTracking switch
        {
            true => baseQuery,
            _ => baseQuery.AsNoTracking(),
        };

        return baseQuery
            .AsSplitQuery()
            .FirstOrDefaultAsync(e => e.Id == userId, cancellationToken);
    }
}
