using MyShop.Application.Dtos.ManagementPanel.MainPageSections;
using MyShop.Core.Models.MainPageSections;

namespace MyShop.Application.Mappings;
internal static class MainPageSectionMappingExtension
{
    public static MainPageSectionMpDto ToMainPageSectionMpDto(
        this MainPageSection entity
        ) => entity switch
        {
            WebsiteHeroSection section => section.ToWebsiteHeroSectionMpDto(),
            WebsiteProductsCarouselSection section => section.ToWebsiteProductCarouselSectionMpDto(),
            _ => throw new NotImplementedException()
        };

    public static WebsiteHeroSectionMpDto ToWebsiteHeroSectionMpDto(
        this WebsiteHeroSection entity
        ) => new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            MainPageSectionType = entity.MainPageSectionType,
            Position = entity.Position,
            DisplayType = entity.DisplayType,
            Label = entity.Label,
        };

    public static WebsiteProductCarouselSectionMpDto ToWebsiteProductCarouselSectionMpDto(
        this WebsiteProductsCarouselSection entity
        ) => new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            MainPageSectionType = entity.MainPageSectionType,
            Position = entity.Position,
            ProductsCarouselSectionType = entity.ProductsCarouselSectionType,
        };

    public static IReadOnlyCollection<MainPageSectionMpDto> ToMainPageSectionMpDtos(
        this IEnumerable<MainPageSection> entities
        ) => entities.Select(ToMainPageSectionMpDto).ToArray();

    public static WebsiteHeroSectionItemMpDto ToWebsiteHeroSectionItemMpDto(
        this WebsiteHeroSectionItem entity
        ) => new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            WebsiteHeroSectionId = entity.WebsiteHeroSectionId,
            Title = entity.Title,
            Subtitle = entity.Subtitle,
            RouterLink = entity.RouterLink,
            Position = entity.Position,
            Photo = entity.WebsiteHeroSectionPhoto.ToPhotoMpDto(),
        };

    public static IReadOnlyCollection<WebsiteHeroSectionItemMpDto> ToWebsiteHeroSectionItemMpDtos(
        this IEnumerable<WebsiteHeroSectionItem> entities
        ) => entities.Select(ToWebsiteHeroSectionItemMpDto).ToArray();
}
