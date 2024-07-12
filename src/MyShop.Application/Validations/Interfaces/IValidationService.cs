using MyShop.Application.Validations.Validators;

namespace MyShop.Application.Validations.Interfaces;
public interface IValidationService
{
    ValidationSummary ValidateModel(IValidatable model);
    ValidationSummary ValidateModels(params IValidatable[] models);
}
