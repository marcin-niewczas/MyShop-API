using MyShop.Application.Dtos.ManagementPanel.Photos;
using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.ManagementPanel.MainPageSections;
public sealed record WebsiteHeroSectionItemMpDto : BaseTimestampDto
{
    public required Guid WebsiteHeroSectionId { get; init; }
    public required string? Title { get; init; }
    public required string? Subtitle { get; init; }
    public required string? RouterLink { get; init; }
    public required int? Position { get; init; }
    public required PhotoMpDto Photo { get; init; }
}
