using MyShop.Core.Validations;

namespace MyShop.Application.Validations.Validators;
public static partial class CustomValidators
{
    public static class PaginationParams
    {
        public static void Validate(
            int? pageNumber,
            int? pageSize,
            ICollection<ValidationMessage> validationMessages,
            string pageNumberParamName = nameof(PageNumber),
            string pageSizeParamName = nameof(PageSize)
            )
        {
            PageNumber.Validate(pageNumber, validationMessages, pageNumberParamName);
            PageSize.Validate(pageSize, validationMessages, pageSizeParamName);
        }

        public static class PageNumber
        {
            public static string ErrorMessage(string paramName = nameof(PageNumber))
                => $"The field {paramName} must be greater than 0.";

            public static void Validate(int? value, ICollection<ValidationMessage> validationMessages, string paramName = nameof(PageNumber))
            {
                if (value is not null && value < 1)
                {
                    validationMessages.Add(new(paramName, [ErrorMessage(paramName)]));
                }
            }
        }

        public static class PageSize
        {
            public const int Min = 1;
            public const int Max = 48;

            public static string ErrorMessage(string paramName = nameof(PageSize))
                => $"The field {paramName} must be inclusive between {Min} and {Max}.";

            public static void Validate(int? value, ICollection<ValidationMessage> validationMessages, string paramName = nameof(PageSize))
            {
                if (value is not null && (value < Min || value > Max))
                {
                    validationMessages.Add(new(paramName, [ErrorMessage(paramName)]));
                }
            }
        }
    }
}
