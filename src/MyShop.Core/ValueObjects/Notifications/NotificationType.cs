using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Core.ValueObjects.Notifications;
public sealed record NotificationType : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues { get; }
        = new string[]
        {
            Order,
            ProductIsAvailable,
            ProductPriceReduced,
            Security,                   
        }.AsReadOnly();

    public const string Order = nameof(Order);
    public const string ProductIsAvailable = nameof(ProductIsAvailable);
    public const string ProductPriceReduced = nameof(ProductPriceReduced);
    public const string Security = nameof(Security);
    

    public string Value { get; }

    public NotificationType(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<NotificationType>());

        Value = value;
    }

    public static implicit operator string(NotificationType value)
        => value.Value;

    public static implicit operator NotificationType(string value)
        => new(value);

    public override string ToString()
        => Value;
}
