using MyShop.Core.Abstractions;
using MyShop.Core.ValueObjects.MainPageSections;

namespace MyShop.Application.Dtos.ValidatorParameters.ManagementPanel;
public sealed record WebsiteHeroSectionItemValidatorParametersMpDto(
    int MaxPosition
    ) : IDto
{
    public StringValidatorParameters TitleParams { get; } = new()
    {
        MinLength = WebsiteHeroSectionItemTitle.MinLength,
        MaxLength = WebsiteHeroSectionItemTitle.MaxLength,
        IsRequired = false
    };
    public StringValidatorParameters SubtitleParams { get; } = new()
    {
        MinLength = WebsiteHeroSectionItemSubtitle.MinLength,
        MaxLength = WebsiteHeroSectionItemSubtitle.MaxLength,
        IsRequired = false
    };
    public StringValidatorParameters RouterLinkParams { get; } = new()
    {
        MinLength = WebsiteHeroSectionItemRouterLink.MinLength,
        MaxLength = WebsiteHeroSectionItemRouterLink.MaxLength,
        IsRequired = false
    };
    public PhotoValidatorParameters PhotoParams { get; }
        = new PhotoValidatorParameters();

    public int MaxActivatedItemsInWebsiteHeroSection { get; }
        = WebsiteHeroSectionItemPosition.Max + 1;
}
