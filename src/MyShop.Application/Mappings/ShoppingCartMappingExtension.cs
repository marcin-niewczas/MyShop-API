using MyShop.Application.Dtos.ECommerce.ShoppingCarts;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.ShoppingCarts;

namespace MyShop.Application.Mappings;
internal static class ShoppingCartMappingExtension
{
    public static CheckoutEcDto ToCheckoutEcDto(this ShoppingCart model)
        => new()
        {
            CheckoutId = model.CheckoutId,
        };

    public static ShoppingCartDetailEcDto ToShoppingCartDetailEcDto(
        this ShoppingCartVerifier model,
        IReadOnlyDictionary<Guid, Changed<int, ShoppingCartItemDetail>> changedDictionary
        )
    {
        var shoppingCartDetailItemDtos = new List<ShoppingCartItemDetailEcDto>();
        Dictionary<Guid, ShoppingCartItemChangedEcDto> changes = [];

        if (changedDictionary.Count > 0 && model.ShoppingCartItemToVerifies.Count <= 0)
        {
            foreach (var item in changedDictionary)
            {
                changes.Add(item.Key, new(
                        item.Value.From,
                        item.Value.To,
                        GetFullProductName(
                            item.Value.Source.ModelName,
                            item.Value.Source.MainDetailOptionValue,
                            item.Value.Source.MainProductVariantOption,
                            item.Value.Source.AdditionalProductVariantOptions)
                            )
                        );
            }
        }
        else
        {
            foreach (var item in model.ShoppingCartItemToVerifies)
            {
                shoppingCartDetailItemDtos.Add(new ShoppingCartItemDetailEcDto
                {
                    ShoppingCartItemId = item.ShoppingCartItem.Id,
                    EncodedName = item.ProductVariant.EncodedName,
                    FullName = $"{item.MainDetailOptionValue} {item.ModelName}",
                    MainProductVariantOption = item.MainProductVariantOption,
                    AdditionalProductVariantOptions = item.AdditionalProductVariantOptions,
                    PhotoUrl = item.MainPhoto?.Uri,
                    PhotoAlt = item.MainPhoto?.Alt,
                    Quantity = item.ShoppingCartItem.Quantity,
                    PricePerEach = item.ProductVariant.Price,
                });

                if (changes.Count < changedDictionary.Count && changedDictionary.TryGetValue(item.ShoppingCartItem.Id, out var change))
                {
                    changes.Add(change.Source.ShoppingCartItem.Id, new(
                        change.From,
                        change.To,
                        GetFullProductName(
                            item.ModelName,
                            item.MainDetailOptionValue,
                            item.MainProductVariantOption,
                            item.AdditionalProductVariantOptions)
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
        OptionNameValue mainProductVariantOptionValue,
        IReadOnlyCollection<OptionNameValue> additionalProductVariantOptionValues
        ) => $"{mainDetailOptionValue} {modelName}{additionalProductVariantOptionValues.Count switch
        {
            <= 0 => string.Empty,
            1 => $" {additionalProductVariantOptionValues.First().Value}",
            > 1 => $" {string.Join("/", additionalProductVariantOptionValues.Select(x => x.Value))}"
        }} ({mainProductVariantOptionValue.Value})";
}
