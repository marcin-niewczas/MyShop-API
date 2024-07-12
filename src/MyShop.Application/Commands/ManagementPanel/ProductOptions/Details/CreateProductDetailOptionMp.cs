using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Application.Commands.ManagementPanel.ProductOptions.Details;
public sealed record CreateProductDetailOptionMp(
    string Name,
    string ProductOptionSubtype,
    string ProductOptionSortType,
    IReadOnlyCollection<string> ProductDetailOptionValues
    ) : ICommand<ApiIdResponse>, 
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        ProductOptionName.Validate(Name, validationMessages);
        CustomValidators.Enums.MustBeIn<ProductOptionSubtype>(ProductOptionSubtype, validationMessages, nameof(ProductOptionSubtype));
        CustomValidators.Enums.MustBeIn<ProductOptionSortType>(ProductOptionSortType, validationMessages, nameof(ProductOptionSortType));
    }
}
