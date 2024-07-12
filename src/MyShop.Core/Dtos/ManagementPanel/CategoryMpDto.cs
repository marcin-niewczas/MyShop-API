using MyShop.Core.Abstractions;

namespace MyShop.Core.Dtos.ManagementPanel;
public sealed record CategoryMpDto : BaseTimestampDto
{
    public required string Name { get; init; }
    public required string HierarchyName { get; init; }
    public required IReadOnlyCollection<CategoryMpDto>? ChildCategories { get; init; }
    public required Guid? ParentCategoryId { get; init; }
    public required Guid RootCategoryId { get; init; }
    public required int Level { get; init; }
}
