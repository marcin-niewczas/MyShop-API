using MyShop.Core.Abstractions;
using MyShop.Core.Models.BaseEntities;

namespace MyShop.Core.Dtos.ManagementPanel;
public sealed record PagedProductMpDto : BaseTimestampDto, IModel
{
    public required string Name { get; init; }
    public required string FullName { get; init; }
    public required CategoryMpDto Category { get; init; }
    public required Guid CategorydId { get; init; }
}
