using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.HelperModels;
using MyShop.Core.Validations;

namespace MyShop.Application.Commands.ManagementPanel.ProductOptionValues.Variants;
public sealed record UpdatePositionsOfProductVariantOptionValuesMp(
    Guid ProductVariantOptionId,
    IReadOnlyCollection<ValuePosition<Guid>> PositionsOfProductVariantOptionValues
    ) : ICommand,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.HelperModels.Validate(
            PositionsOfProductVariantOptionValues,
            validationMessages,
            paramName: nameof(PositionsOfProductVariantOptionValues)
            );
    }
}
