using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;
using MyShop.Core.Validations;

namespace MyShop.Application.Validations.Validators;
public static partial class CustomValidators
{
    public static class Enums
    {
        public static string GetEnumErrorMessage<TEnum>(
            string paramName
            ) where TEnum : struct, Enum
            => $"The field {paramName} must be in [ {string.Join(", ", Enum.GetNames<TEnum>())} ].";

        public static void IsInEnum<TEnum>(
            string? value,
            ICollection<ValidationMessage> validationMessages,
            string paramName,
            bool isNullable = false
            ) where TEnum : struct, Enum
        {
            if (value is null && isNullable)
            {
                return;
            }

            if (value is null || !Enum.TryParse<TEnum>(value, out _))
            {
                validationMessages.Add(new(paramName, [$"The field {paramName} must be in [ {string.Join(", ", Enum.GetNames<TEnum>())} ]."]));
            }
        }

        public static void MustBeIn<TAllowedValues>(
            object value,
            ICollection<ValidationMessage> validationMessages,
            string paramName
            ) where TAllowedValues : IAllowedValues
        {
            if (!TAllowedValues.AllowedValues.Any(x => x.Equals(value)))
            {
                validationMessages.Add(new(paramName, [AllowedValuesError.Message<TAllowedValues>()]));
            }
        }

        public static void MustBeIn(
            string value,
            IReadOnlyCollection<string> allowedValues,
            ICollection<ValidationMessage> validationMessages,
            string paramName
            )
        {
            if (!allowedValues.Contains(value))
            {
                validationMessages.Add(new(paramName, [$"The field {paramName} must be in [ {string.Join(", ", allowedValues)} ]."]));
            }
        }

        public static void MustBeNullOrIn<TAllowedValues>(
            object? value,
            ICollection<ValidationMessage> validationMessages,
            string paramName
            ) where TAllowedValues : IAllowedValues
        {
            if (value is null)
            {
                return;
            }

            MustBeIn<TAllowedValues>(value, validationMessages, paramName);
        }
    }
}
