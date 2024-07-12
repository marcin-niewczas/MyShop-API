using MyShop.Application.Validations.Validators;
using MyShop.Core.RepositoryQueryParams.Commons;

namespace MyShop.Application.Mappings;
public static class TypeMapper
{
    public static SortDirection? MapOptionalSortDirection(string? sortDirection)
        => sortDirection switch
        {
            null => null,
            { Length: 9 or 10 } => Enum.Parse<SortDirection>(sortDirection),
            _ => throw new ArgumentException(CustomValidators.SortParams.SortDirection.ErrorMessage(nameof(sortDirection)))
        };

    public static TEnum MapEnum<TEnum>(string value) where TEnum : struct, Enum
        => value switch
        {
            not null => Enum.Parse<TEnum>(value),
            null => throw new ArgumentException(value, nameof(value))
        };

    public static TEnum? MapOptionalEnum<TEnum>(string? value) where TEnum : struct, Enum
        => value switch
        {
            not null => Enum.Parse<TEnum>(value),
            null => null
        };
}
