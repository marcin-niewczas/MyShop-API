using MyShop.Core.Validations;

namespace MyShop.Core.ValueObjects.Interfaces;
public interface IValidatableValueObject
{
    static abstract void Validate(object? value, ICollection<ValidationMessage> validationMessages);
}
