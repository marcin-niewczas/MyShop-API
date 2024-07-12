using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Photos;
using MyShop.Core.ValueObjects.MainPageSections;

namespace MyShop.Core.Models.MainPageSections;
public sealed class WebsiteHeroSectionItem : BaseTimestampEntity
{
    public WebsiteHeroSection WebsiteHeroSection { get; private set; } = default!;
    public Guid WebsiteHeroSectionId { get; private set; }
    public WebsiteHeroSectionPhoto WebsiteHeroSectionPhoto { get; private set; } = default!;
    public Guid WebsiteHeroSectionPhotoId { get; private set; }
    public WebsiteHeroSectionItemPosition Position { get; private set; } = default!;
    public WebsiteHeroSectionItemTitle Title { get; private set; } = default!;
    public WebsiteHeroSectionItemSubtitle Subtitle { get; private set; } = default!;
    public WebsiteHeroSectionItemRouterLink RouterLink { get; private set; } = default!;


    private WebsiteHeroSectionItem() { }

    public WebsiteHeroSectionItem(
        WebsiteHeroSection websiteHeroSection,
        WebsiteHeroSectionPhoto websiteHeroSectionPhoto,
        WebsiteHeroSectionItemPosition position,
        WebsiteHeroSectionItemTitle title,
        WebsiteHeroSectionItemSubtitle subtitle,
        WebsiteHeroSectionItemRouterLink routerLink
        )
    {
        WebsiteHeroSection = websiteHeroSection ?? throw new ArgumentNullException(nameof(websiteHeroSection));
        WebsiteHeroSectionId = websiteHeroSection.Id;
        WebsiteHeroSectionPhoto = websiteHeroSectionPhoto ?? throw new ArgumentNullException(nameof(websiteHeroSectionPhoto));
        WebsiteHeroSectionPhotoId = websiteHeroSectionPhoto.Id;
        Position = position ?? throw new ArgumentNullException(nameof(position));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Subtitle = subtitle ?? throw new ArgumentNullException(nameof(subtitle));
        RouterLink = routerLink ?? throw new ArgumentNullException(nameof(routerLink));
    }

    public void Update(
        WebsiteHeroSectionItemTitle title,
        WebsiteHeroSectionItemSubtitle subtitle,
        WebsiteHeroSectionItemRouterLink routerLink,
        WebsiteHeroSectionPhoto websiteHeroSectionPhoto
        )
    {
        ArgumentNullException.ThrowIfNull(title, nameof(title));
        ArgumentNullException.ThrowIfNull(subtitle, nameof(subtitle));
        ArgumentNullException.ThrowIfNull(routerLink, nameof(routerLink));
        ArgumentNullException.ThrowIfNull(websiteHeroSectionPhoto, nameof(websiteHeroSectionPhoto));

        if (Title == title &&
            Subtitle == subtitle &&
            RouterLink == routerLink &&
            websiteHeroSectionPhoto.Id == WebsiteHeroSectionPhoto.Id)
        {
            throw new BadRequestException($"Nothing change in {nameof(WebsiteHeroSectionItem)}.");
        }

        Title = title;
        Subtitle = subtitle;
        RouterLink = routerLink;
        WebsiteHeroSectionPhoto = websiteHeroSectionPhoto;
    }

    public void UpdatePosition(WebsiteHeroSectionItemPosition position)
    {
        ArgumentNullException.ThrowIfNull(nameof(position));

        if (Position == position)
        {
            throw new BadRequestException($"The {nameof(Position)} is same for {nameof(WebsiteHeroSectionItem)} '{Id}'.");
        }

        Position = position;
    }
}
