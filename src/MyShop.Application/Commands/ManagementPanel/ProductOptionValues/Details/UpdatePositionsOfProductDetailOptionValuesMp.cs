using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.HelperModels;
using MyShop.Core.Validations;

namespace MyShop.Application.Commands.ManagementPanel.ProductOptionValues.Details;
public sealed record UpdatePositionsOfProductDetailOptionValuesMp(
    Guid ProductDetailOptionId,
    IReadOnlyCollection<ValuePosition<Guid>> PositionsOfProductDetailOptionValues
    ) : ICommand, IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.HelperModels.Validate(
            PositionsOfProductDetailOptionValues,
            validationMessages,
            paramName: nameof(PositionsOfProductDetailOptionValues)
            );
    }
}
