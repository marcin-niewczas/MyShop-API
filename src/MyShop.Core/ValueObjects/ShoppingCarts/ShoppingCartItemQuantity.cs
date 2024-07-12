using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;
using MyShop.Core.ValueObjects.Products;

namespace MyShop.Core.ValueObjects.ShoppingCarts;
public sealed record ShoppingCartItemQuantity : IValidatableValueObject
{
    public int Value { get; }
    public ShoppingCartItemQuantity(int value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), GetErrorMessage());
        }

        Value = value;
    }

    public static implicit operator int(ShoppingCartItemQuantity value)
        => value.Value;

    public static implicit operator ShoppingCartItemQuantity(int value)
        => new(value);

    public static implicit operator ProductVariantQuantity(ShoppingCartItemQuantity value)
        => new(value);

    public static implicit operator ShoppingCartItemQuantity(ProductVariantQuantity value)
        => new(value.Value);

    public const int Min = 1;
    public const int Max = 10;

    public static bool IsValid(int value)
       => value is >= Min and <= Max;

    public override string ToString()
        => Value.ToString();

    private static string GetErrorMessage()
        => $"The {nameof(ShoppingCartItemQuantity)} must be inclusvie between {Min} and {Max}.";

    private static string GetErrorMessage(int value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is int intValue)
        {
            if (!IsValid(intValue))
            {
                validationMessages.Add(new(nameof(ShoppingCartItemQuantity), [GetErrorMessage(intValue)]));
            }
        }
        else
        {
            validationMessages.Add(
                new(
                    nameof(ShoppingCartItemQuantity),
                    [$"The {nameof(ShoppingCartItemQuantity)} must be an int.", GetErrorMessage()]
                    )
                );
        }
    }
}
