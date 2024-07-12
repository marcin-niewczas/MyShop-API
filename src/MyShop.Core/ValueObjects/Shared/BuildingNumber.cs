using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;

namespace MyShop.Core.ValueObjects.Shared;
public sealed record BuildingNumber : IValidatableValueObject
{
    public string Value { get; }
    public BuildingNumber(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException(GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator string(BuildingNumber value)
        => value.Value;

    public static implicit operator BuildingNumber(string value)
        => new(value);

    public override string ToString()
        => Value;

    public const int MinLength = 1;
    public const int MaxLength = 50;

    private static bool IsValid(string value)
        => !string.IsNullOrWhiteSpace(value) && value.Length is >= MinLength and <= MaxLength;

    private static string GetErrorMessage()
        => $"The {nameof(BuildingNumber)} must be between {MinLength} and {MaxLength} and not be whitespace.";

    private static string GetErrorMessage(string value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is string stringValue)
        {
            if (!IsValid(stringValue))
            {
                validationMessages.Add(new(nameof(BuildingNumber), [GetErrorMessage(stringValue)]));
            }
        }
        else
        {
            validationMessages.Add(
                new(
                    nameof(BuildingNumber),
                    [$"The {nameof(BuildingNumber)} must be a string.", GetErrorMessage()]
                    )
                );
        }
    }
}
