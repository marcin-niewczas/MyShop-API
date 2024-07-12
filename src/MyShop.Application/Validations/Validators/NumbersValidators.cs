using MyShop.Core.Validations;
using System.Numerics;

namespace MyShop.Application.Validations.Validators;
public static partial class CustomValidators
{
    public static class Numbers
    {
        public static void MustBePositive<TNumber>(
            TNumber value,
            ICollection<ValidationMessage> validationMessages,
            string paramName
            ) where TNumber : struct, INumberBase<TNumber>
        {
            if (!TNumber.IsPositive(value))
            {
                validationMessages.Add(new(paramName, [$"The field {paramName} must be positive number."]));
            }
        }

        public static void MustBeNullOrPositive<TNumber>(
            TNumber? value,
            ICollection<ValidationMessage> validationMessages,
            string paramName
            ) where TNumber : struct, INumberBase<TNumber>
        {
            if (value is TNumber v && !TNumber.IsPositive(v))
            {
                validationMessages.Add(new(paramName, [$"The field {paramName} must be skipped or positive number."]));
            }
        }
    }
}
