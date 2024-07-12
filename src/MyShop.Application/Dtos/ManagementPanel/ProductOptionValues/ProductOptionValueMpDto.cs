using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.ManagementPanel.ProductOptionValues;
public sealed record ProductOptionValueMpDto : BaseTimestampDto
{
    public required string Value { get; init; }
    public required Guid ProductOptionId { get; init; }
}
