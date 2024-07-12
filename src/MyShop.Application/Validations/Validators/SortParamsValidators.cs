using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Core.Validations;
using RepositoryQueryParamsCommons = MyShop.Core.RepositoryQueryParams.Commons;

namespace MyShop.Application.Validations.Validators;
public static partial class CustomValidators
{
    public static class SortParams
    {
        public static void Validate<TSortByEnum>(
            string? sortBy,
            string? sortDirection,
            ICollection<ValidationMessage> validationMessages,
            string sortByParamName = nameof(ISortQueryParams.SortBy),
            string sortDirectionParamName = nameof(SortDirection)
            ) where TSortByEnum : struct, Enum
        {
            if (sortBy is null && sortDirection is not null)
            {
                validationMessages.Add(new(
                    sortByParamName,
                    [$"The field {sortByParamName} must be in [ {string.Join(", ", Enum.GetNames<TSortByEnum>())} ], if the field {sortDirectionParamName} isn't skipped."]
                    ));
            }

            if (sortBy is not null && sortDirection is null)
            {
                validationMessages.Add(new(
                    sortDirectionParamName,
                    [$"The field {sortDirectionParamName} must be in [ {string.Join(", ", SortDirection.AllowedValues)} ], if the field {sortByParamName} isn't skipped."]
                    ));
            }

            Enums.IsInEnum<TSortByEnum>(sortBy, validationMessages, sortDirectionParamName, isNullable: true);
            SortDirection.Validate(sortDirection, validationMessages, sortDirectionParamName);
        }

        public static class SortDirection
        {
            public readonly static string[] AllowedValues = Enum.GetNames<RepositoryQueryParamsCommons.SortDirection>();

            public static string ErrorMessage(
                string paramName = nameof(ISortQueryParams.SortDirection)
                ) => $"The field {paramName} must be in [ {string.Join(", ", AllowedValues)} ].";

            public static void Validate(
                string? value,
                ICollection<ValidationMessage> validationMessages,
                string paramName = nameof(SortDirection)
                )
            {
                if (value is not null && !AllowedValues.Contains(value))
                {
                    validationMessages.Add(new(paramName, [$"The field {paramName} must be in [ {string.Join(", ", AllowedValues)} ]."]));
                }
            }
        }
    }
}
