using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Core.ValueObjects.Orders;
public sealed record PaymentMethod : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues { get; }
        = new string[]
        {
            ApplePay,
            CashOnDelivery,
            GooglePay,
            MyShopPay,
            PayPal,
        }.AsReadOnly();

    public const string CashOnDelivery = "Cash On Delivery";
    public const string PayPal = nameof(PayPal);
    public const string MyShopPay = "myShop Pay";
    public const string GooglePay = "Google Pay";
    public const string ApplePay = "Apple Pay";

    public string Value { get; }

    public PaymentMethod(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<PaymentMethod>());

        Value = value;
    }

    public static implicit operator string(PaymentMethod value)
        => value.Value;

    public static implicit operator PaymentMethod(string value)
        => new(value);

    public override string ToString()
        => Value;
}
