using MyShop.Application.Dtos.ManagementPanel.ProductOptions;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Application.Commands.ManagementPanel.ProductOptions.Details;
public sealed record UpdateProductDetailOptionMp(
    Guid Id,
    string Name,
    string ProductOptionSortType
    ) : ICommand<ApiResponse<ProductOptionMpDto>>, IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        ProductOptionName.Validate(Name, validationMessages);
        CustomValidators.Enums.MustBeIn<ProductOptionSortType>(
            ProductOptionSortType,
            validationMessages,
            nameof(ProductOptionSortType)
            );
    }
}
