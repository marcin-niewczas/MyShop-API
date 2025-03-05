using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;

namespace MyShop.Application.Mappings;
internal static class ProductVariantMappingExtension
{
    public static PagedProductVariantMpDto ToPagedProductVariantMpDto(this ProductVariant entity)
        => new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Quantity = entity.Quantity,
            Price = entity.Price,
            EncodedName = entity.EncodedName,
            SkuId = entity.SkuId,
            ProductVariantValues = entity.Product
                        .ProductProductVariantOptions
                        .Join(entity.ProductVariantOptionValues,
                              k => k.ProductVariantOptionId,
                              k => k.ProductOptionId,
                              (option, value) => new OptionNameValueId(
                                  option.ProductVariantOptionId,
                                  option.ProductVariantOption.Name,
                                  value.Value
                                  )).ToArray()
        };

    public static IReadOnlyCollection<PagedProductVariantMpDto> ToPagedProductVariantMpDtos(
        this IEnumerable<ProductVariant> entities
        ) => entities.Select(ToPagedProductVariantMpDto).ToArray();


}
