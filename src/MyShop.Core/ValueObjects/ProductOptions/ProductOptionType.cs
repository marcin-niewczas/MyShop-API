using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Core.ValueObjects.ProductOptions;
public sealed record ProductOptionType : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues { get; }
        = new string[]
        {
            Variant,
            Detail
        }.AsReadOnly();

    public string Value { get; }

    public const string Variant = nameof(Variant);
    public const string Detail = nameof(Detail);

    public ProductOptionType(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<ProductOptionType>());

        Value = value;
    }

    public static implicit operator string(ProductOptionType value)
        => value.Value;

    public static implicit operator ProductOptionType(string value)
        => new(value);

    public override string ToString()
        => Value;
}
