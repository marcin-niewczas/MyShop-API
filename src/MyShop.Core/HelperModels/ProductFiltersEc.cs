using MyShop.Core.Models.BaseEntities;

namespace MyShop.Core.HelperModels;
public sealed record ProductFiltersEc : IModel
{
    public required decimal? MinPrice { get; init; }
    public required decimal? MaxPrice { get; init; }
    public required IReadOnlyCollection<ProductOptionEc> ProductOptions { get; init; }
}

public sealed record ProductOptionEc
{
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string Subtype { get; init; }
    public required IReadOnlyCollection<ProductOptionValueEc> Values { get; init; }
}

public sealed record ProductOptionValueEc(string Value, int Count);
