using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;

namespace MyShop.Core.ValueObjects.Shared;
public sealed record Email : IValidatableValueObject
{
    public string Value { get; }
    public Email(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException(GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator string(Email value)
        => value.Value;

    public static implicit operator Email(string value)
        => new(value);

    public override string ToString()
        => Value;

    public const int MinLength = 1;
    public const int MaxLength = 250;

    private static bool IsValid(string value)
        => !string.IsNullOrWhiteSpace(value) &&
            value.Length is >= MinLength and <= MaxLength &&
            CustomRegex.Email().IsMatch(value);

    private static string GetErrorMessage()
        => $"The {nameof(Email)} must be between {MinLength} and {MaxLength} and have correct format.";

    private static string GetErrorMessage(string value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is string stringValue)
        {
            if (!IsValid(stringValue))
            {
                validationMessages.Add(new(nameof(Email), [GetErrorMessage(stringValue)]));
            }
        }
        else
        {
            validationMessages.Add(
                new(
                    nameof(Email),
                    [$"The {nameof(Email)} must be a string.", GetErrorMessage()]
                    )
                );
        }
    }
}
