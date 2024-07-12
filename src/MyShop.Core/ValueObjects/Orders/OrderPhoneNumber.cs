using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;
using MyShop.Core.ValueObjects.Users;

namespace MyShop.Core.ValueObjects.Orders;
public sealed record OrderPhoneNumber : IValidatableValueObject
{
    public string Value { get; }
    public OrderPhoneNumber(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException(GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator string(OrderPhoneNumber value)
        => value.Value;

    public static implicit operator OrderPhoneNumber(string value)
        => new(value);

    public override string ToString()
        => Value;

    public const int MinLength = UserPhoneNumber.MinLength;
    public const int MaxLength = UserPhoneNumber.MaxLength;

    private static bool IsValid(string value)
        => !string.IsNullOrWhiteSpace(value) &&
            value.Length is >= MinLength and <= MaxLength &&
            CustomRegex.PhoneNumber().IsMatch(value);

    private static string GetErrorMessage()
        => $"The {nameof(OrderPhoneNumber)} must be between {MinLength} and {MaxLength} and have correct format.";

    private static string GetErrorMessage(string value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is string stringValue)
        {
            if (!IsValid(stringValue))
            {
                validationMessages.Add(new(nameof(OrderPhoneNumber), [GetErrorMessage(stringValue)]));
            }
        }
        else
        {
            validationMessages.Add(
                new(
                    nameof(OrderPhoneNumber),
                    [$"The {nameof(OrderPhoneNumber)} must be a string.", GetErrorMessage()]
                    )
                );
        }
    }
}
