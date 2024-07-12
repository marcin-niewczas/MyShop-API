using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Core.ValueObjects.Orders;
public sealed record OrderStatus : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues { get; }
        = new string[]
        {
            New,
            WaitingForPayment,
            PaymentReceived,
            PaymentFailed,
            Shipped,
            Delivered,
            Completed,
            Canceled
        }.AsReadOnly();

    public const string New = nameof(New);
    public const string WaitingForPayment = "Waiting For Payment";
    public const string PaymentReceived = "Payment Received";
    public const string PaymentFailed = "Payment Failed";
    public const string Shipped = "Shipped";
    public const string Delivered = "Delivered";
    public const string Completed = nameof(Completed);
    public const string Canceled = nameof(Canceled);

    public string Value { get; }

    public bool CanBeCancelled()
        => Value is New or WaitingForPayment;

    public OrderStatus(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<OrderStatus>());

        Value = value;
    }

    public static implicit operator string(OrderStatus value)
        => value.Value;

    public static implicit operator OrderStatus(string value)
        => new(value);

    public override string ToString()
        => Value;

    public static IReadOnlyCollection<string> GetAvailableOrderStatusToUpdate()
        => new string[]
        {
            WaitingForPayment,
            PaymentReceived,
            PaymentFailed,
            Shipped,
            Delivered,
            Completed,
            Canceled
        }.AsReadOnly();
}
