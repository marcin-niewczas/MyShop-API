using MyShop.Core.HelperModels;
using MyShop.Core.Utils;
using MyShop.Core.Validations;

namespace MyShop.Application.Validations.Validators;
public static partial class CustomValidators
{
    public static class HelperModels
    {
        public static void Validate<TValue>(
            IReadOnlyCollection<ValuePosition<TValue>> idPositions,
            ICollection<ValidationMessage> validationMessages,
            bool onlyPositivePosition = false,
            int? maxPosition = null,
            bool isRequired = true,
            string paramName = $"{nameof(ValuePosition<TValue>)}s"
            )
        {
            if (idPositions.IsNullOrEmpty())
            {
                if (!isRequired)
                {
                    return;
                }

                validationMessages.Add(new(paramName, [$"The collection {paramName} is required."]));
                return;
            }

            if (idPositions.HasDuplicateBy(x => x.Value))
            {
                validationMessages.Add(new(paramName, [$"The collection {paramName} must contains unique {nameof(ValuePosition<TValue>.Value)}s."]));
            }

            if (idPositions.HasDuplicateBy(x => x.Position) || idPositions.Any(x => onlyPositivePosition ? x.Position <= 0 : x.Position < 0))
            {
                validationMessages.Add(new(
                    paramName,
                    [$"The collection {paramName} must contains {(onlyPositivePosition ? "greater than" : "greater than or equals to")} 0 unique {nameof(ValuePosition<TValue>.Position)}s."]
                    ));
            }

            if (maxPosition is not null)
            {
                if (idPositions.Any(x => x.Position > maxPosition))
                {
                    validationMessages.Add(new(
                        paramName,
                        [$"The collection {paramName} must contains {nameof(ValuePosition<TValue>.Position)}s not greater than {maxPosition}."]
                        ));
                }
            }
        }
    }
}
