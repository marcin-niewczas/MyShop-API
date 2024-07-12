using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;
using MyShop.Core.ValueObjects.Products;
using MyShop.Core.ValueObjects.ShoppingCarts;

namespace MyShop.Core.ValueObjects.Orders;
public sealed record OrderProductQuantity : IValidatableValueObject
{
    public int Value { get; }
    public OrderProductQuantity(int value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator int(OrderProductQuantity value)
        => value.Value;

    public static implicit operator OrderProductQuantity(int value)
        => new(value);

    public static implicit operator OrderProductQuantity(ShoppingCartItemQuantity value)
        => new(value.Value);

    public static implicit operator ShoppingCartItemQuantity(OrderProductQuantity value)
        => new(value);

    public static implicit operator OrderProductQuantity(ProductVariantQuantity value)
        => new(value.Value);

    public static implicit operator ProductVariantQuantity(OrderProductQuantity value)
        => new(value);

    public override string ToString()
        => Value.ToString();

    public const int Min = 1;
    public const int Max = ShoppingCartItemQuantity.Max;

    public static bool IsValid(int value)
        => value is >= Min and <= Max;

    private static string GetErrorMessage()
        => $"The {nameof(OrderProductQuantity)} must be inclusive between {Min} and {Max}.";

    private static string GetErrorMessage(int value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is int intValue)
        {
            if (!IsValid(intValue))
            {
                validationMessages.Add(new(nameof(OrderProductQuantity), [GetErrorMessage(intValue)]));
            }
        }
        else
        {
            validationMessages.Add(
                new(
                    nameof(OrderProductQuantity),
                    [$"The {nameof(OrderProductQuantity)} must be a int.", GetErrorMessage()]
                    )
                );
        }
    }
}
