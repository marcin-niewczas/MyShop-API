using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;

namespace MyShop.Core.ValueObjects.Users;
public sealed record UserPhoneNumber : IValidatableValueObject
{
    public string? Value { get; }
    public UserPhoneNumber(string? value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException(GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator string?(UserPhoneNumber value)
        => value?.Value;

    public static implicit operator UserPhoneNumber(string? value)
        => new(value);

    public override string? ToString()
        => Value;

    public const int MinLength = 1;
    public const int MaxLength = 100;

    private static bool IsValid(string? value)
        => string.IsNullOrWhiteSpace(value) ||
            value.Length is >= MinLength and <= MaxLength &&
            CustomRegex.PhoneNumber().IsMatch(value);

    private static string GetErrorMessage()
        => $"The {nameof(UserPhoneNumber)} must be between {MinLength} and {MaxLength} and have correct format.";

    private static string GetErrorMessage(string? value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is not null)
        {
            if (value is string stringValue)
            {
                if (!IsValid(stringValue))
                {
                    validationMessages.Add(new(nameof(UserPhoneNumber), [GetErrorMessage(stringValue)]));
                }
            }
            else
            {
                validationMessages.Add(
                    new(
                        nameof(UserPhoneNumber),
                        [$"The {nameof(UserPhoneNumber)} must be a string.", GetErrorMessage()]
                        )
                    );
            }
        }
    }
}
