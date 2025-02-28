using MyShop.Application.Dtos.ECommerce.ShoppingCarts;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Photos;
using MyShop.Core.Models.Products;
using MyShop.Core.Models.ShoppingCarts;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Application.Mappings;
internal static class ShoppingCartMappingExtension
{
    public static CheckoutEcDto ToCheckoutEcDto(this ShoppingCart model)
        => new()
        {
            CheckoutId = model.CheckoutId,
        };

    public static ShoppingCartDetailEcDto ToShoppingCartDetailEcDto(
        this ShoppingCart model,
        IReadOnlyDictionary<Guid, Changed<int, ShoppingCartItem>> changedDictionary
        )
    {
        var shoppingCartDetailItemDtos = new List<ShoppingCartItemDetailEcDto>();
        Dictionary<Guid, ShoppingCartItemChangedEcDto> changes = [];

        if (changedDictionary.Count > 0 && model.ShoppingCartItems.Count <= 0)
        {
            foreach (var item in changedDictionary)
            {
                changes.Add(item.Key, new(
                        item.Value.From,
                        item.Value.To,
                        GetFullProductName(
                            item.Value.Source.ProductVariant.Product.Name,
                            item.Value.Source.ProductVariant.Product.ProductDetailOptionValues.First().Value,
                            item.Value.Source.ProductVariant.ProductVariantOptionValues.First(v => v.ProductVariantOption.ProductOptionSubtype == ProductOptionSubtype.Main),
                            GetSortedAdditionalProductVariantOptionValues(item.Value.Source.ProductVariant)
                            )
                        )
                    );
            }
        }
        else
        {
            ProductVariantPhoto? tempPhoto;

            foreach (var item in model.ShoppingCartItems)
            {
                tempPhoto = item.ProductVariant.Photos.FirstOrDefault();

                shoppingCartDetailItemDtos.Add(new ShoppingCartItemDetailEcDto
                {
                    ShoppingCartItemId = item.Id,
                    EncodedName = item.ProductVariant.EncodedName,
                    FullName = $"{item.ProductVariant.Product.ProductDetailOptionValues.First().Value} {item.ProductVariant.Product.Name}",
                    MainProductVariantOption = item.ProductVariant.ProductVariantOptionValues.Select(x => new OptionNameValue(x.ProductVariantOption.Name, x.Value)).First(),
                    AdditionalProductVariantOptions = GetSortedAdditionalProductVariantOptionValues(item.ProductVariant).Select(x => new OptionNameValue(x.ProductVariantOption.Name, x.Value)).ToArray(),
                    PhotoUrl = tempPhoto?.Uri,
                    PhotoAlt = tempPhoto?.Alt,
                    Quantity = item.Quantity,
                    PricePerEach = item.ProductVariant.Price,
                });

                if (changes.Count < changedDictionary.Count && changedDictionary.TryGetValue(item.Id, out var change))
                {
                    changes.Add(change.Source.Id, new(
                        change.From,
                        change.To,
                        GetFullProductName(
                            item.ProductVariant.Product.Name,
                            item.ProductVariant.Product.ProductDetailOptionValues.First().Value,
                            item.ProductVariant.ProductVariantOptionValues.First(v => v.ProductVariantOption.ProductOptionSubtype == ProductOptionSubtype.Main),
                            GetSortedAdditionalProductVariantOptionValues(item.ProductVariant))
                            )
                        );
                }
            }
        }

        return new()
        {
            Changes = changes.Count is > 0 ? changes : null,
            ShoppingCartItems = shoppingCartDetailItemDtos,
        };
    }

    private static string GetFullProductName(
        string modelName,
        string mainDetailOptionValue,
        BaseProductOptionValue mainProductVariantOptionValue,
        IReadOnlyCollection<BaseProductOptionValue> additionalProductVariantOptionValues
        ) => $"{mainDetailOptionValue} {modelName}{additionalProductVariantOptionValues.Count switch
        {
            <= 0 => string.Empty,
            1 => $" {additionalProductVariantOptionValues.First().Value}",
            > 1 => $" {string.Join("/", additionalProductVariantOptionValues.Select(x => x.Value))}"
        }} ({mainProductVariantOptionValue.Value})";

    private static ProductVariantOptionValue[] GetSortedAdditionalProductVariantOptionValues(ProductVariant productVariant)
    {
        return productVariant.ProductVariantOptionValues
            .Where(v => v.ProductVariantOption.ProductOptionSubtype == ProductOptionSubtype.Additional)
            .Join(
                productVariant.Product.ProductProductVariantOptions,
                k => k.ProductOptionId,
                k => k.ProductVariantOptionId,
                (pvov, ppvo) => new { Pos = ppvo.Position, ProductVariantOptionValue = pvov }
            )
            .OrderBy(o => (int)o.Pos)
            .Select(x => x.ProductVariantOptionValue)
            .ToArray();
    }
}
