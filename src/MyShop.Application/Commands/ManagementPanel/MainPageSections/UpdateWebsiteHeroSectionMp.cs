using MyShop.Application.Validations.Interfaces;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.MainPageSections;

namespace MyShop.Application.Commands.ManagementPanel.MainPageSections;
public sealed record UpdateWebsiteHeroSectionMp(
    Guid Id,
    string Label
    ) : ICommand, IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        WebsiteHeroSectionLabel.Validate(Label, validationMessages);
    }
}
