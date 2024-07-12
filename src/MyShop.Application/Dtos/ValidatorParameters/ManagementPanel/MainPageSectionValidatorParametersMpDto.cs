using MyShop.Core.Abstractions;
using MyShop.Core.ValueObjects.MainPageSections;

namespace MyShop.Application.Dtos.ValidatorParameters.ManagementPanel;
public sealed record MainPageSectionValidatorParametersMpDto : IDto
{
    public IReadOnlyCollection<string> ProductsCarouselSectionTypes { get; }
        = ProductsCarouselSectionType
            .AllowedValues
            .Cast<string>()
            .ToArray();

    public IReadOnlyCollection<string> WebsiteHeroSectionDisplayTypes { get; }
        = WebsiteHeroSectionDisplayType
            .AllowedValues
            .Cast<string>()
            .ToArray();

    public StringValidatorParameters WebsiteHeroSectionLabelParams { get; } = new()
    {
        MinLength = WebsiteHeroSectionLabel.MinLength,
        MaxLength = WebsiteHeroSectionLabel.MaxLength,
    };

    public int MaxMainPageSections { get; }
       = MainPageSectionPosition.Max + 1;

    public int MaxActivatedItemsInWebsiteHeroSection { get; }
        = WebsiteHeroSectionItemPosition.Max + 1;
}
