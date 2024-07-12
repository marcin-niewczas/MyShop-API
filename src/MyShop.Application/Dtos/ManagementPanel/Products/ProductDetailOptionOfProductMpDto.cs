using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.ManagementPanel.Products;
public sealed record ProductDetailOptionOfProductMpDto : BaseTimestampDto
{
    public required Guid ProductOptionId { get; init; }
    public required Guid ProductOptionValueId { get; init; }
    public required string ProductOptionSubtype { get; init; }
    public required string Name { get; init; }
    public required string Value { get; init; }
}
