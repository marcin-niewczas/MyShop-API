using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.MainPageSections;

namespace MyShop.Application.Commands.ManagementPanel.MainPageSections;
public sealed record CreateWebsiteProductCarouselSectionMp(
    string ProductsCarouselSectionType
    ) : ICommand<ApiIdResponse>,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators
            .Enums
            .MustBeIn<ProductsCarouselSectionType>(
                ProductsCarouselSectionType,
                validationMessages,
                nameof(ProductsCarouselSectionType)
                );
    }
}
