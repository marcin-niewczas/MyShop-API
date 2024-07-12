using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ValueObjects.Shared;
public sealed record Password : IValidatableValueObject
{
    public string Value { get; }
    public Password(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException(GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator string(Password value)
        => value.Value;

    public static implicit operator Password(string value)
        => new(value);

    public override string ToString()
        => Value;

    public const string MinLength = "8";
    public const string MaxLength = "255";

    private static bool IsValid(string value)
        => CustomRegex.Password().IsMatch(value);

    public static string GetErrorMessage()
        => $"The {nameof(Password)} must have minimum {MinLength} characters, maximum {MaxLength} characters, " +
        $"at least one lower case English letter, one upper case English letter, one number and one special character.";

    private static string GetErrorMessage(string value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is string stringValue)
        {
            if (!IsValid(stringValue))
            {
                validationMessages.Add(new(nameof(Password), [GetErrorMessage(stringValue)]));
            }
        }
        else
        {
            validationMessages.Add(
                new(
                    nameof(Password),
                    [$"The {nameof(Password)} must be a string.", GetErrorMessage()]
                    )
                );
        }
    }
}
