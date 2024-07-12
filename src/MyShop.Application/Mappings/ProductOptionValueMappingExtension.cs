using MyShop.Application.Dtos.ManagementPanel.ProductOptionValues;
using MyShop.Core.Models.Products;

namespace MyShop.Application.Mappings;
internal static class ProductOptionValueMappingExtension
{
    public static ProductOptionValueMpDto ToProductOptionValueMpDto(this BaseProductOptionValue entity)
        => new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Value = entity.Value,
            ProductOptionId = entity.ProductOptionId
        };

    public static IReadOnlyCollection<ProductOptionValueMpDto> ToProductOptionValueMpDtos(
        this IEnumerable<BaseProductOptionValue> entities
        ) => entities.Select(ToProductOptionValueMpDto).ToArray();
}
