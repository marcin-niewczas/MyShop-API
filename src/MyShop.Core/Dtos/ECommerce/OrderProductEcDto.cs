using MyShop.Core.Abstractions;
using MyShop.Core.Dtos.Shared;
using MyShop.Core.HelperModels;

namespace MyShop.Core.Dtos.ECommerce;
public sealed record OrderProductEcDto : IDto
{
    public required string Name { get; init; }
    public required string CategoryHierarchyName { get; init; }
    public required string EncodedName { get; init; }
    public required PhotoDto? MainPhoto { get; init; }
    public required int Quantity { get; init; }
    public required decimal Price { get; init; }
    public required Guid OrderId { get; init; }
    public required decimal PriceAll { get; init; }
    public required IReadOnlyCollection<OptionNameValue> VariantOptionNameValues { get; init; }
}
