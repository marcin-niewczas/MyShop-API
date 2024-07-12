using Microsoft.AspNetCore.Mvc;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.HelperModels;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Products;

namespace MyShop.Application.Commands.ManagementPanel.ProductVariants;
public sealed record UpdatePositionsOfProductVariantPhotoItemsMp(
    [FromRoute] Guid Id,
    [FromBody] IReadOnlyCollection<ValuePosition<Guid>> IdPositions
    ) : ICommand, IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.HelperModels.Validate(
            IdPositions,
            validationMessages,
            maxPosition: ProductVariantPhotoItemPosition.Max,
            paramName: nameof(IdPositions)
            );
    }
}
