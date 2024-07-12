using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Infrastructure.Notifications;
public sealed record NotificationSenderType : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues { get; } =
        [
            Sms,
            Email,
            Hub
        ];

    public const string Sms = nameof(Sms);
    public const string Email = nameof(Email);
    public const string Hub = nameof(Hub);

    public string Value { get; }

    public NotificationSenderType(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<NotificationSenderType>());

        Value = value;
    }

    public static implicit operator string(NotificationSenderType value)
        => value.Value;

    public static implicit operator NotificationSenderType(string value)
        => new(value);

    public override string ToString()
        => Value;
}
