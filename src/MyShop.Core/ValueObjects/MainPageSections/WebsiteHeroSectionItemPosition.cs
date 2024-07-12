using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;

namespace MyShop.Core.ValueObjects.MainPageSections;
public sealed record WebsiteHeroSectionItemPosition : IValidatableValueObject
{
    public int? Value { get; }
    public WebsiteHeroSectionItemPosition(int? value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException(GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator int?(WebsiteHeroSectionItemPosition value)
        => value?.Value;

    public static implicit operator WebsiteHeroSectionItemPosition(int? value)
        => new(value);

    public override string? ToString()
        => Value?.ToString();

    public const int Min = 0;
    public const int Max = 4;

    private static bool IsValid(int? value)
        => value is null or >= Min and <= Max;

    private static string GetErrorMessage()
        => $"The {nameof(WebsiteHeroSectionItemPosition)} isn't required, but if entered must be inclusive between {Min} and {Max}.";

    private static string GetErrorMessage(int? value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is not null)
        {
            if (value is int intValue)
            {
                if (!IsValid(intValue))
                {
                    validationMessages.Add(new(nameof(WebsiteHeroSectionItemPosition), [GetErrorMessage(intValue)]));
                }
            }
            else
            {
                validationMessages.Add(
                    new(
                        nameof(WebsiteHeroSectionItemPosition),
                        [$"The {nameof(WebsiteHeroSectionItemPosition)} is not required.", GetErrorMessage()]
                        )
                    );
            }
        }
    }
}
