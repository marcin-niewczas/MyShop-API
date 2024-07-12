using MyShop.Core.Abstractions;
using System.Text.Json.Serialization;

namespace MyShop.Application.Dtos.ManagementPanel.MainPageSections;

[JsonDerivedType(typeof(WebsiteHeroSectionMpDto)), JsonDerivedType(typeof(WebsiteProductCarouselSectionMpDto))]
public abstract record MainPageSectionMpDto : BaseTimestampDto
{
    public required string MainPageSectionType { get; init; }
    public required int Position { get; set; }
}

public sealed record WebsiteHeroSectionMpDto : MainPageSectionMpDto
{
    public required string Label { get; init; }
    public required string DisplayType { get; init; }
}

public sealed record WebsiteProductCarouselSectionMpDto : MainPageSectionMpDto
{
    public required string ProductsCarouselSectionType { get; init; }
}
