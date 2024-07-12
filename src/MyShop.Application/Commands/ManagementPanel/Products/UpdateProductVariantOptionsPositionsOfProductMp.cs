using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.HelperModels;
using MyShop.Core.Validations;

namespace MyShop.Application.Commands.ManagementPanel.Products;
public sealed record UpdateProductVariantOptionsPositionsOfProductMp(
    Guid ProductId,
    IReadOnlyCollection<ValuePosition<Guid>> IdPositions
    ) : ICommand, 
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.HelperModels.Validate(
            IdPositions,
            validationMessages,
            onlyPositivePosition: true,
            paramName: nameof(IdPositions)
            );
    }
}
