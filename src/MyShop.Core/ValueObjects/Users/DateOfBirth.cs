using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ValueObjects.Users;
public sealed record DateOfBirth : IValidatableValueObject
{
    public DateOnly Value { get; }
    public DateOfBirth(DateOnly value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException(GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator DateOnly(DateOfBirth value)
        => value.Value;

    public static implicit operator DateOfBirth(DateOnly value)
        => new(value);

    public override string ToString()
        => Value.ToString();

    public static readonly DateOnly Min = new(1850, 1, 1);
    public static DateOnly Max
        => DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-10);

    private static bool IsValid(DateOnly value)
        => value >= Min && value <= Max;

    private static string GetErrorMessage()
        => $"The {nameof(DateOfBirth)} must be between {Min} and {Max}.";

    private static string GetErrorMessage(DateOnly value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is DateOnly dateOnlyValue)
        {
            if (!IsValid(dateOnlyValue))
            {
                validationMessages.Add(new(nameof(DateOfBirth), [GetErrorMessage(dateOnlyValue)]));
            }
        }
        else
        {
            validationMessages.Add(
                new(
                    nameof(DateOfBirth),
                    [$"The {nameof(DateOfBirth)} must be a {Min.GetType().Name}.", GetErrorMessage()]
                    )
                );
        }
    }
}
