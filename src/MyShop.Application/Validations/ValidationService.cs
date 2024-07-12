using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;

namespace MyShop.Application.Validations;
public sealed class ValidationService : IValidationService
{
    public ValidationSummary ValidateModel(IValidatable model)
    {
        ArgumentNullException.ThrowIfNull(nameof(model));

        var validationMessages = new List<ValidationMessage>();

        model.Validate(validationMessages);

        return new(validationMessages);
    }

    public ValidationSummary ValidateModels(params IValidatable[] models)
    {
        ArgumentNullException.ThrowIfNull(nameof(models));

        var validationMessages = new List<ValidationMessage>();

        foreach (var model in models)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(models), "Model cannot be null.");
            }

            model.Validate(validationMessages);
        }

        return new(validationMessages);
    }
}
