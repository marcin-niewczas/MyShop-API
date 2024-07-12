using Microsoft.EntityFrameworkCore;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.ShoppingCarts;
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

        var shoppingCart = _dbSet
            .Where(e => e.UserId == userId)
            .Select(x => new ShoppingCartVerifier(
               x.ShoppingCartItems.Select(i => new ShoppingCartItemDetail(
                    i.ProductVariant.ProductVariantOptionValues.AsQueryable().Include(i => i.ProductVariantOption).First(v => v.ProductVariantOption.ProductOptionSubtype == ProductOptionSubtype.Main),
                    i.ProductVariant.Photos.FirstOrDefault(p => p.ProductVariantPhotoItems.Any(i => i.Position == 0))
                    )

               {
                   ShoppingCartItem = i,
                   ProductVariant = i.ProductVariant,
                   AdditionalProductVariantOptions = i.ProductVariant.Product.ProductProductVariantOptions
                    .Where(v => v.ProductVariantOption.ProductOptionSubtype == ProductOptionSubtype.Additional)
                    .OrderBy(o => o.Position)
                    .Join(
                        i.ProductVariant.ProductVariantOptionValues,
                        k => k.ProductVariantOptionId,
                        k => k.ProductOptionId,
                        (_, v) => new OptionNameValue(v.ProductVariantOption.Name, v.Value)
                        )
                    .ToList(),
                   ModelName = i.ProductVariant.Product.Name,
                   MainDetailOptionValue = i.ProductVariant
                      .Product
                      .ProductDetailOptionValues
                      .First(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main).Value
               }).ToList()
            )
            { ShoppingCart = x });

        return await shoppingCart
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);
    }
}
