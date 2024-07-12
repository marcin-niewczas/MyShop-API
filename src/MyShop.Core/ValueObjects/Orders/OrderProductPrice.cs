using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;
using MyShop.Core.ValueObjects.Products;

namespace MyShop.Core.ValueObjects.Orders;
public sealed record OrderProductPrice : IValidatableValueObject
{
    public decimal Value { get; }
    public OrderProductPrice(decimal value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator decimal(OrderProductPrice value)
        => value.Value;

    public static implicit operator OrderProductPrice(decimal value)
        => new(value);

    public static implicit operator ProductVariantPrice(OrderProductPrice value)
        => new(value);

    public static implicit operator OrderProductPrice(ProductVariantPrice value)
        => new(value.Value);

    public const int Min = 0;

    public static bool IsValid(decimal value)
        => value is >= Min;

    public override string ToString()
        => Value.ToString();

    private static string GetErrorMessage()
        => $"The {nameof(OrderProductPrice)} must be greater than or equal to {Min}.";

    private static string GetErrorMessage(decimal value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is decimal decimalValue)
        {
            if (!IsValid(decimalValue))
            {
                validationMessages.Add(new(nameof(OrderProductPrice), [GetErrorMessage(decimalValue)]));
            }
        }
        else
        {
            validationMessages.Add(
                new(
                    nameof(OrderProductPrice),
                    [$"The {nameof(OrderProductPrice)} must be a decimal.", GetErrorMessage()]
                    )
                );
        }
    }
}
