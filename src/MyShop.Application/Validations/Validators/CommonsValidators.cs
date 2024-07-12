using MyShop.Core.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Application.Validations.Validators;
public static partial class CustomValidators
{
    public static class Commons
    {
        public static void MustBeIn<TValue>(
            TValue value,
            IEnumerable<TValue> allowedValues,
            ICollection<ValidationMessage> validationMessages,
            string paramName
            ) where TValue : struct
        {
            if (!allowedValues.Contains(value))
            {
                validationMessages.Add(new(paramName, [$"The field {paramName} must be in [ {string.Join(", ", allowedValues)} ]."]));
            }
        }
    }
}
