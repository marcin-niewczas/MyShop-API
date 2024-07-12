using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ValueObjects.Shared;
public sealed record FirstName : IValidatableValueObject
{
    public string Value { get; }
    public FirstName(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException(GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator string(FirstName value)
        => value.Value;

    public static implicit operator FirstName(string value)
        => new(value);

    public override string ToString()
        => Value;

    public const int MinLength = 3;
    public const int MaxLength = 255;

    private static bool IsValid(string value)
        => !string.IsNullOrWhiteSpace(value) && value.Length is >= MinLength and <= MaxLength;

    private static string GetErrorMessage()
        => $"The {nameof(FirstName)} must be between {MinLength} and {MaxLength} and not be whitespace.";

    private static string GetErrorMessage(string value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is string stringValue)
        {
            if (!IsValid(stringValue))
            {
                validationMessages.Add(new(nameof(FirstName), [GetErrorMessage(stringValue)]));
            }
        }
        else
        {
            validationMessages.Add(
                new(
                    nameof(FirstName),
                    [$"The {nameof(FirstName)} must be a string.", GetErrorMessage()]
                    )
                );
        }
    }
}
