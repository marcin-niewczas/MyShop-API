using MyShop.Core.Abstractions;
using MyShop.Core.Dtos.Shared;
using MyShop.Core.Models.BaseEntities;
using System.Text.Json.Serialization;

namespace MyShop.Core.Dtos.ECommerce;
[JsonDerivedType(typeof(WebsiteHeroSectionEcDto)), JsonDerivedType(typeof(WebsiteProductsCarouselSectionEcDto))]
public abstract record MainPageSectionEcDto : IDto, IModel
{
    public required string MainPageSectionType { get; init; }
}

public sealed record WebsiteHeroSectionEcDto : MainPageSectionEcDto
{
    public required string DisplayType { get; init; }
    public required IReadOnlyCollection<WebsiteHeroSectionItemEcDto> Items { get; init; }
}

public sealed record WebsiteHeroSectionItemEcDto : IDto
{
    public required string? Title { get; init; }
    public required string? Subtitle { get; init; }
    public required string? RouterLink { get; init; }
    public required PhotoDto Photo { get; init; }
}

public sealed record WebsiteProductsCarouselSectionEcDto : MainPageSectionEcDto
{
    public required string ProductsCarouselSectionType { get; init; }
    public IReadOnlyCollection<ProductItemDto> Items { get; private set; } = default!;

    public void SetItems(IReadOnlyCollection<ProductItemDto> items)
    {
        ArgumentNullException.ThrowIfNull(nameof(items));
        Items = items;
    }
}
