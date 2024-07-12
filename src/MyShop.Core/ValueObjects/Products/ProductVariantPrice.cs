using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ValueObjects.Products;
public sealed record ProductVariantPrice : IValidatableValueObject
{
    public decimal Value { get; }
    public ProductVariantPrice(decimal value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator decimal(ProductVariantPrice value)
        => value.Value;

    public static implicit operator ProductVariantPrice(decimal value)
        => new(value);

    public const int Min = 0;

    public static bool IsValid(decimal value)
        => value is >= Min;

    public override string ToString()
        => Value.ToString();

    private static string GetErrorMessage()
        => $"The {nameof(ProductVariantPrice)} must be greater than or equal to {Min}.";

    private static string GetErrorMessage(decimal value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is decimal decimalValue)
        {
            if (!IsValid(decimalValue))
            {
                validationMessages.Add(new(nameof(ProductName), [GetErrorMessage(decimalValue)]));
            }
        }
        else
        {
            validationMessages.Add(
                new(
                    nameof(ProductVariantPrice),
                    [$"The {nameof(ProductVariantPrice)} must be a decimal.", GetErrorMessage()]
                    )
                );
        }
    }
}
