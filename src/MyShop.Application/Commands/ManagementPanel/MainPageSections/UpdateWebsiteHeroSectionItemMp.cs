using Microsoft.AspNetCore.Http;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.MainPageSections;
using MyShop.Core.ValueObjects.Photos;

namespace MyShop.Application.Commands.ManagementPanel.MainPageSections;
public sealed record UpdateWebsiteHeroSectionItemMp : ICommand, IValidatable
{
    public Guid Id { get; init; }
    public string? Title { get; init; }
    public string? Subtitle { get; init; }
    public string? RouterLink { get; init; }
    public Guid? WebsiteHeroSectionPhotoId { get; init; }
    public IFormFile? WebsiteHeroSectionItemPhoto { get; init; }

    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        if (WebsiteHeroSectionPhotoId is null && WebsiteHeroSectionItemPhoto is null)
        {
            validationMessages.Add(new(
            "*",
                [$"The field {nameof(WebsiteHeroSectionPhotoId)} cannot be null, if the field {nameof(WebsiteHeroSectionItemPhoto)} is null and vice versa."])
                );
        }

        if (WebsiteHeroSectionPhotoId is not null && WebsiteHeroSectionItemPhoto is not null)
        {
            validationMessages.Add(new(
                "*",
                [$"Only one of {nameof(WebsiteHeroSectionPhotoId)} and {nameof(WebsiteHeroSectionItemPhoto)} field is required."])
                );
        }

        if (WebsiteHeroSectionItemPhoto is not null)
        {
            CustomValidators.Photos.Validate(
                WebsiteHeroSectionItemPhoto,
                PhotoType.WebsiteHeroPhoto,
                validationMessages,
                nameof(WebsiteHeroSectionItemPhoto),
                true
                );
        }

        WebsiteHeroSectionItemTitle.Validate(Title, validationMessages);
        WebsiteHeroSectionItemSubtitle.Validate(Subtitle, validationMessages);
        WebsiteHeroSectionItemRouterLink.Validate(RouterLink, validationMessages);
    }
}
