using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.ManagementPanel.Products;
public sealed record ProductVariantOptionOfProductMpDto : BaseTimestampDto
{
    public required Guid ProductOptionId { get; init; }
    public required string ProductOptionSubtype { get; init; }
    public required string Name { get; init; }
}
