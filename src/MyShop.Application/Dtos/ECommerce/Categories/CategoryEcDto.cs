using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.ECommerce.Categories;
public sealed record CategoryEcDto : BaseDto
{
    public required string Name { get; init; }
    public required bool IsRoot { get; init; }
    public required string HierarchyName { get; init; }
    public required string EncodedHierarchyName { get; init; }
    public required IReadOnlyCollection<CategoryEcDto>? ChildCategories { get; init; }
}
