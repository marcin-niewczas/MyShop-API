using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;

namespace MyShop.Core.ValueObjects.Photos;
public sealed record PhotoAlt : IValidatableValueObject
{
    public string Value { get; }
    public PhotoAlt(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException(GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator string(PhotoAlt? value)
        => value?.Value!;

    public static implicit operator PhotoAlt(string value)
        => new(value);

    public override string ToString()
        => Value;

    public const int MinLength = 1;
    public const int MaxLength = 2000;

    private static bool IsValid(string value)
        => !string.IsNullOrWhiteSpace(value) && value.Length is >= MinLength and <= MaxLength;

    private static string GetErrorMessage()
        => $"The {nameof(PhotoAlt)} must be between {MinLength} and {MaxLength} and not be whitespace.";

    private static string GetErrorMessage(string value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is string stringValue)
        {
            if (!IsValid(stringValue))
            {
                validationMessages.Add(new(nameof(PhotoAlt), [GetErrorMessage(stringValue)]));
            }
        }
        else
        {
            validationMessages.Add(
                new(
                    nameof(PhotoAlt),
                    [$"The {nameof(PhotoAlt)} must be a string.", GetErrorMessage()]
                    )
                );
        }
    }
}
