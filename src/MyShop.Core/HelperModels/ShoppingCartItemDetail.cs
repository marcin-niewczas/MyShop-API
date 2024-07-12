using MyShop.Core.Models.Photos;
using MyShop.Core.Models.Products;
using MyShop.Core.Models.ShoppingCarts;

namespace MyShop.Core.HelperModels;
public sealed record ShoppingCartItemDetail
{
    public required ProductVariant ProductVariant { get; init; }
    public required ShoppingCartItem ShoppingCartItem { get; init; }
    public required string ModelName { get; init; }
    public required string MainDetailOptionValue { get; init; }
    public OptionNameValue MainProductVariantOption { get; }
    public required IReadOnlyCollection<OptionNameValue> AdditionalProductVariantOptions { get; init; }
    public Photo? MainPhoto { get; }

    public ShoppingCartItemDetail(
        ProductVariantOptionValue mainProductVariantOptionValue,
        Photo? mainPhoto
        )
    {
        MainProductVariantOption = new OptionNameValue(
            mainProductVariantOptionValue.ProductVariantOption.Name,
            mainProductVariantOptionValue.Value
            );
        MainPhoto = mainPhoto;
    }
}
