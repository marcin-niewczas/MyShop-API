using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ValueObjects.Photos;
public sealed record PhotoSize : IValidatableValueObject
{
    public decimal ValueinKilobytes { get; }
    public PhotoSize(decimal value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), GetErrorMessage(value));
        }

        ValueinKilobytes = value;
    }

    public static implicit operator decimal(PhotoSize value)
        => value.ValueinKilobytes;

    public static implicit operator PhotoSize(decimal value)
        => new(value);

    public const int MaxSizeInMegabytes = 5;
    public const int Min = 0;
    public const int Max = MaxSizeInMegabytes * 1024;

    public static bool IsValid(decimal value)
        => value is >= Min and <= Max;

    public override string ToString()
        => ValueinKilobytes.ToString();

    private static string GetErrorMessage()
        => $"The {nameof(PhotoSize)} must be inclusive between {Min} and {Max}.";

    private static string GetErrorMessage(decimal value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is decimal decimalValue)
        {
            if (!IsValid(decimalValue))
            {
                validationMessages.Add(new(nameof(PhotoSize), [GetErrorMessage(decimalValue)]));
            }
        }
        else
        {
            validationMessages.Add(
                new(
                    nameof(PhotoSize),
                    [$"The {nameof(PhotoSize)} must be a decimal.", GetErrorMessage()]
                    )
                );
        }
    }
}
