using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace MyShop.Core.ValueObjects.MainPageSections;
public sealed record WebsiteHeroSectionItemTitle : IValidatableValueObject
{
    public string? Value { get; }
    public WebsiteHeroSectionItemTitle(string? value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException(GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator string?(WebsiteHeroSectionItemTitle value)
        => value?.Value;

    public static implicit operator WebsiteHeroSectionItemTitle(string? value)
        => new(value);

    public override string? ToString()
        => Value;

    public const int MinLength = 1;
    public const int MaxLength = 64;

    private static bool IsValid([NotNullWhen(false)] string? value)
        => value is null || (value.Trim().Length is not 0 && value.Length is >= MinLength and <= MaxLength);

    private static string GetErrorMessage()
        => $"The {nameof(WebsiteHeroSectionItemTitle)} must be between {MinLength} and {MaxLength} and not be whitespace.";

    private static string GetErrorMessage(string value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is not null)
        {
            if (value is string stringValue)
            {
                if (!IsValid(stringValue))
                {
                    validationMessages.Add(new(nameof(WebsiteHeroSectionItemTitle), [GetErrorMessage(stringValue)]));
                }
            }
            else
            {
                validationMessages.Add(
                    new(
                        nameof(WebsiteHeroSectionItemTitle),
                        [$"The {nameof(WebsiteHeroSectionItemTitle)} must be a string.", GetErrorMessage()]
                        )
                    );
            }
        }
    }
}
