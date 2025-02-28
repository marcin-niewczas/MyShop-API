using Microsoft.EntityFrameworkCore;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.ShoppingCarts;
using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class ShoppingCartRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<ShoppingCart>(dbContext), IShoppingCartRepository
{

    public Task<ShoppingCart?> GetUserShoppingCartWithItemsAndProductVariantsAsync(
        Guid userId,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        )
    {
        var baseQuery = _dbSet
            .Include(i => i.ShoppingCartItems)
            .ThenInclude(i => i.ProductVariant)
            .AsQueryable();

        baseQuery = withTracking switch
        {
            true => baseQuery,
            _ => baseQuery.AsNoTracking(),
        };


        return baseQuery
            .FirstOrDefaultAsync(e => e.UserId == userId, cancellationToken);
    }

    public async Task<ShoppingCartVerifier?> GetShoppingCartVerifierAsync(
        Guid userId,
        CancellationToken cancellationToken = default
        )
    {
        var shoppingCart = await _dbSet
            .Include(i => i.ShoppingCartItems)
            .ThenInclude(i => i.ProductVariant)
            .ThenInclude(i => i.Product)
            .ThenInclude(i => i.ProductProductVariantOptions)
            .Include(i => i.ShoppingCartItems)
            .ThenInclude(i => i.ProductVariant)
            .ThenInclude(i => i.ProductVariantOptionValues)
            .ThenInclude(i => i.ProductVariantOption)
            .Include(i => i.ShoppingCartItems)
            .ThenInclude(i => i.ProductVariant)
            .ThenInclude(i => i.Product)
            .ThenInclude(i => i.ProductDetailOptionValues.Where(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main))
            .Include(i => i.ShoppingCartItems)
            .ThenInclude(i => i.ProductVariant)
            .ThenInclude(i => i.PhotoItems.OrderBy(o => o.Position).Take(1))
            .ThenInclude(i => i.ProductVariantPhoto)
            .AsSplitQuery()
            .FirstOrDefaultAsync(e => e.UserId == userId, cancellationToken)
                ?? throw new InvalidDataInDatabaseException(
                    $"The {nameof(ShoppingCart)} for {nameof(User)} with {nameof(IEntity.Id)} equal '{userId}' not exist."
                    );

        return new(shoppingCart);
    }
}
