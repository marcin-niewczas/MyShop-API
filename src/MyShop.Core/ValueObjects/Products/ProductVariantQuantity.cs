using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ValueObjects.Products;
public sealed record ProductVariantQuantity : IValidatableValueObject
{
    public int Value { get; }
    public ProductVariantQuantity(int value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator int(ProductVariantQuantity value)
        => value.Value;

    public static implicit operator ProductVariantQuantity(int value)
        => new(value);

    public const int Min = 0;

    public static bool IsValid(int value)
        => value is >= Min;

    public override string ToString()
        => Value.ToString();

    public bool? IsLastItemsInStock()
        => Value <= 0
            ? null
            : Value <= 3;

    private static string GetErrorMessage()
        => $"The {nameof(ProductVariantQuantity)} must be greater than or equal to {Min}.";

    private static string GetErrorMessage(int value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is int intValue)
        {
            if (!IsValid(intValue))
            {
                validationMessages.Add(new(nameof(ProductVariantQuantity), [GetErrorMessage(intValue)]));
            }
        }
        else
        {
            validationMessages.Add(
                new(
                    nameof(ProductVariantPrice),
                    [$"The {nameof(ProductVariantQuantity)} must be a int.", GetErrorMessage()]
                    )
                );
        }
    }
}
