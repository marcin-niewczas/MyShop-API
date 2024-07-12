using MyShop.Application.Dtos.ECommerce.Products;
using MyShop.Application.Dtos.ManagementPanel.Products;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;

namespace MyShop.Application.Mappings;
internal static class ProductMappingExtension
{
    public static ProductFiltersDtoEc ToProductFiltersEcDto(this ProductFiltersEc model, Category category)
        => new()
        {
            Category = category.ToCategoryEcDto(),
            MinPrice = model.MinPrice,
            MaxPrice = model.MaxPrice,
            ProductOptions = model.ProductOptions,
        };

    public static ProductDetailOptionOfProductMpDto ToProductDetailOptionOfProductMpDto(this ProductProductDetailOptionValue model)
        => new()
        {
            Id = model.Id,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            Name = model.ProductDetailOptionValue.ProductDetailOption.Name,
            Value = model.ProductDetailOptionValue.Value,
            ProductOptionSubtype = model.ProductDetailOptionValue.ProductDetailOption.ProductOptionSubtype,
            ProductOptionId = model.ProductDetailOptionValue.ProductDetailOption.Id,
            ProductOptionValueId = model.ProductDetailOptionValue.Id
        };

    public static IReadOnlyCollection<ProductDetailOptionOfProductMpDto> ToProductDetailOptionOfProductMpDtos(
        this IEnumerable<ProductProductDetailOptionValue> entities
        ) => entities.Select(ToProductDetailOptionOfProductMpDto).ToArray();

    public static ProductVariantOptionOfProductMpDto ToProductVariantOptionOfProductMpDto(this ProductProductVariantOption model)
        => new()
        {
            Id = model.Id,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            Name = model.ProductVariantOption.Name,
            ProductOptionSubtype = model.ProductVariantOption.ProductOptionSubtype,
            ProductOptionId = model.ProductVariantOption.Id,
        };

    public static IReadOnlyCollection<ProductVariantOptionOfProductMpDto> ToProductVariantOptionOfProductMpDtos(
        this IEnumerable<ProductProductVariantOption> entities
        ) => entities.Select(ToProductVariantOptionOfProductMpDto).ToArray();
}
