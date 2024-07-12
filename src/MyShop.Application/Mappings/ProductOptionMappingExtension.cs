using MyShop.Application.Dtos.ManagementPanel.ProductOptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.Mappings;
internal static class ProductOptionMappingExtension
{
    public static ProductOptionMpDto ToProductOptionMpDto(this BaseProductOption entity)
        => new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Name = entity.Name,
            ProductOptionType = entity.ProductOptionType,
            ProductOptionSubtype = entity.ProductOptionSubtype,
            ProductOptionSortType = entity.ProductOptionSortType,
            ProductOptionValues = entity.ProductOptionValues?.ToProductOptionValueMpDtos(),
        };

    public static IReadOnlyCollection<ProductOptionMpDto> ToProductOptionMpDtos(this IEnumerable<BaseProductOption> entities)
        => entities.Select(ToProductOptionMpDto).ToArray();
}
