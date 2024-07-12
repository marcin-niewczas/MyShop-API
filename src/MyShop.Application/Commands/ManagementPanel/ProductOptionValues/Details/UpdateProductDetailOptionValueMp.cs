using MyShop.Application.Dtos.ManagementPanel.ProductOptionValues;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Application.Commands.ManagementPanel.ProductOptionValues.Details;
public sealed record UpdateProductDetailOptionValueMp(
    Guid Id,
    string Value
    ) : ICommand<ApiResponse<ProductOptionValueMpDto>>, IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        ProductOptionValue.Validate(Value, validationMessages);
    }
}
