using MyShop.Core.Abstractions;
using MyShop.Core.HelperModels;

namespace MyShop.Core.Dtos.ManagementPanel;
public sealed record ProductMpDto : BaseTimestampDto
{
    public required string Name { get; init; }
    public required string FullName { get; init; }
    public required string DisplayProductType { get; init; }
    public required string? Description { get; init; }
    public required CategoryMpDto Category { get; init; }
    public required Guid CategorydId { get; init; }
    public required IReadOnlyCollection<OptionNameValueId> ProductDetailOptions { get; init; }
    public required IReadOnlyCollection<OptionNameId> ProductVariantOptions { get; init; }
}
