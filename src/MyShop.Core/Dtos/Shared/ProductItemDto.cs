using MyShop.Core.Abstractions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.ValueObjects.Products;

namespace MyShop.Core.Dtos.Shared;
public sealed record ProductItemDto : IModel, IDto
{
    public required ProductDataDto ProductData { get; init; }
    public bool IsStablePrice
        => MinPrice == MaxPrice;
    public required decimal MinPrice { get; init; }
    public required decimal MaxPrice { get; init; }
    public required int VariantsCount { get; init; }
    public required bool IsAvailable { get; init; }
    public required int ProductReviewsCount { get; init; }
    public required double ProductReviewsRate { get; init; }
}

public sealed record ProductDataDto
{
    public string FullName =>
        $"{MainDetailOptionValue} {(DisplayProductPer == DisplayProductType.AllVariantOptions) switch
        {
            true => $"{ModelName} {VariantLabel}",
            _ => ModelName
        }}";
    public required string ModelName { get; init; }
    public required Guid? ProductVariantId { get; init; }
    public required string CategoryHierarchyName { get; init; }
    public required string DisplayProductPer { get; init; }
    public required string EncodedName { get; init; }
    public required string MainDetailOptionValue { get; init; }
    public required string MainVariantOptionValue { get; init; }
    public required bool HasMultipleVariants { get; init; }
    public required string VariantLabel { get; init; }
    public required PhotoDto? MainPhoto { get; init; }
}
