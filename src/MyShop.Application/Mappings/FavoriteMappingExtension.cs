using MyShop.Application.Dtos.ECommerce.Favorites;
using MyShop.Core.Models.Products;

namespace MyShop.Application.Mappings;
public static class FavoriteMappingExtension
{
    public static StatusOfFavoritesDictionaryEcDto ToStatusOfFavoritesDictionaryEcDto(
        this IReadOnlyCollection<Favorite> favorites,
        IReadOnlyCollection<string> requestedEncodedProductNames
        ) => favorites.Count switch
        {
            <= 0 => new(requestedEncodedProductNames.ToDictionary(k => k, v => false)),
            _ => new(requestedEncodedProductNames.ToDictionary(k => k, v => favorites.Any(e => e.EncodedProductVariantName == v)))
        };

}
