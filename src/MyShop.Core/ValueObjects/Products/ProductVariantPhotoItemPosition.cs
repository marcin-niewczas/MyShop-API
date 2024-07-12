using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ValueObjects.Products;
public sealed record ProductVariantPhotoItemPosition : IValidatableValueObject
{
    public int Value { get; }
    public ProductVariantPhotoItemPosition(int value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator int(ProductVariantPhotoItemPosition value)
        => value.Value;

    public static implicit operator ProductVariantPhotoItemPosition(int value)
        => new(value);

    public override string ToString()
        => Value.ToString();

    public const int Min = 0;
    public const int Max = 3;

    public static bool IsValid(int value)
        => value is >= Min and <= Max;

    public static string GetErrorMessage()
        => $"The {nameof(ProductVariantPhotoItemPosition)} must be inclusvie between {Min} and {Max}.";

    private static string GetErrorMessage(int value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is int intValue)
        {
            if (!IsValid(intValue))
            {
                validationMessages.Add(new(nameof(ProductVariantPhotoItemPosition), [GetErrorMessage(intValue)]));
            }
        }
        else
        {
            validationMessages.Add(
                new(
                    nameof(ProductVariantPhotoItemPosition),
                    [$"The {nameof(ProductVariantPhotoItemPosition)} must be a int.", GetErrorMessage()]
                    )
                );
        }
    }
}
