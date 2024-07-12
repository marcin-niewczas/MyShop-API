using MyShop.Application.Dtos.ManagementPanel.ProductVariantPhotoItems;
using MyShop.Core.Models.Products;

namespace MyShop.Application.Mappings;
internal static class ProductVariantPhotoItemMappingExtension
{
    public static ProductVariantPhotoItemMpDto ToProductVariantPhotoItemMpDto(this ProductVariantPhotoItem entity)
        => new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Position = entity.Position,
            Photo = entity.ProductVariantPhoto.ToPhotoMpDto(),
        };

    public static IReadOnlyCollection<ProductVariantPhotoItemMpDto> ToProductVariantPhotoItemMpDtos(
        this IEnumerable<ProductVariantPhotoItem> entities
        ) => entities.Select(ToProductVariantPhotoItemMpDto).ToArray();
}
