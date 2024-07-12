using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Core.ValueObjects.Orders;
public sealed class DeliveryMethod : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues { get; }
        = new string[]
        {
            DHL,
            FedEx,
            UPS,
        }.AsReadOnly();

    public const string DHL = nameof(DHL);
    public const string FedEx = nameof(FedEx);
    public const string UPS = nameof(UPS);

    public string Value { get; }

    public DeliveryMethod(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<DeliveryMethod>());

        Value = value;
    }

    public static implicit operator string(DeliveryMethod value)
        => value.Value;

    public static implicit operator DeliveryMethod(string value)
        => new(value);

    public override string ToString()
        => Value;
}
