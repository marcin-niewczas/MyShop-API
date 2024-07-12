using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.MainPageSections;

namespace MyShop.Application.Commands.ManagementPanel.MainPageSections;
public sealed record CreateWebsiteHeroSectionMp(
    string Label,
    string DisplayType
    ) : ICommand<ApiIdResponse>, 
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        WebsiteHeroSectionLabel.Validate(Label, validationMessages);
        CustomValidators.Enums.MustBeIn<WebsiteHeroSectionDisplayType>(DisplayType, validationMessages, nameof(DisplayType));
    }
}
