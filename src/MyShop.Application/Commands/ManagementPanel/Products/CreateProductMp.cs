using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Core.HelperModels;
using MyShop.Core.Utils;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Products;

namespace MyShop.Application.Commands.ManagementPanel.Products;
public sealed record CreateProductMp(
    string Name,
    string DisplayProductPer,
    string Description,
    Guid ProductCategoryId,
    IReadOnlyCollection<ValuePosition<Guid>> ChosenProductDetailOptionValues,
    IReadOnlyCollection<ValuePosition<Guid>> ChosenProductVariantOptions
    ) : ICommand<ApiIdResponse>,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        ProductName.Validate(Name, validationMessages);
        Validations.Validators.CustomValidators.Enums.MustBeIn<DisplayProductType>(
            DisplayProductPer,
            validationMessages,
            nameof(DisplayProductPer)
            );
        ProductDescription.Validate(Description, validationMessages);

        if (ChosenProductDetailOptionValues.IsNullOrEmpty())
        {
            validationMessages.Add(new(
                nameof(ChosenProductDetailOptionValues),
                [$"The field {nameof(ChosenProductDetailOptionValues)} is required."]
                ));
        }

        if (ChosenProductVariantOptions.IsNullOrEmpty())
        {
            validationMessages.Add(new(
                nameof(ChosenProductVariantOptions),
                [$"The field {nameof(ChosenProductVariantOptions)} is required."]
                ));
        }
    }
}
