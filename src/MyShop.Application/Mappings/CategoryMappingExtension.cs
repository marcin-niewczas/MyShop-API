using MyShop.Application.Dtos.ECommerce.Categories;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Models.Products;

namespace MyShop.Application.Mappings;
internal static class CategoryMappingExtension
{
    public static CategoryMpDto ToCategoryMpDto(this Category entity)
        => new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Name = entity.Name,
            HierarchyName = entity.HierarchyDetail.HierarchyName,
            ChildCategories = entity.ChildCategories?.ToCategoryMpDtos(),
            ParentCategoryId = entity.ParentCategoryId,
            RootCategoryId = entity.HierarchyDetail.RootCategoryId,
            Level = entity.HierarchyDetail.Level
        };

    public static IReadOnlyCollection<CategoryMpDto> ToCategoryMpDtos(this IEnumerable<Category> entities)
        => entities.Select(ToCategoryMpDto).ToArray();

    public static CategoryEcDto ToCategoryEcDto(this Category entity)
        => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            IsRoot = entity.ParentCategoryId is null,
            HierarchyName = entity.HierarchyDetail.HierarchyName,
            EncodedHierarchyName = entity.HierarchyDetail.EncodedHierarchyName,
            ChildCategories = entity.ChildCategories?.ToCategoryEcDtos(),
        };

    public static IReadOnlyCollection<CategoryEcDto> ToCategoryEcDtos(this IEnumerable<Category> entities)
        => entities.Select(ToCategoryEcDto).ToArray();
}
