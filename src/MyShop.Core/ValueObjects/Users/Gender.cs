using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Core.ValueObjects.Users;
public sealed record Gender : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues { get; }
        = new string[]
        {
            Female,
            Male
        }.AsReadOnly();

    public string Value { get; private init; } = default!;

    public const string Female = nameof(Female);
    public const string Male = nameof(Male);

    public Gender(string value)
    {
        if (value is null || !AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<Gender>());

        Value = value;
    }

    public static implicit operator string(Gender value)
        => value.Value;

    public static implicit operator Gender(string value)
        => new(value);

    public override string ToString()
        => Value;
}
