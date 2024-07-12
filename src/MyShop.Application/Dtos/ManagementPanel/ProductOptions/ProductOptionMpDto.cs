using MyShop.Application.Dtos.ManagementPanel.ProductOptionValues;
using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.ManagementPanel.ProductOptions;
public sealed record ProductOptionMpDto : BaseTimestampDto
{
    public required string Name { get; init; }
    public required string ProductOptionSubtype { get; init; }
    public required string ProductOptionType { get; init; }
    public required string ProductOptionSortType { get; init; }
    public required IReadOnlyCollection<ProductOptionValueMpDto>? ProductOptionValues { get; init; }
}
