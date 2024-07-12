using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Core.ValueObjects.ProductOptions;
public sealed record ProductOptionSubtype : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues { get; }
        = new string[]
        {
            Main,
            Additional
        }.AsReadOnly();

    public string Value { get; }

    public const string Main = nameof(Main);
    public const string Additional = nameof(Additional);

    public ProductOptionSubtype(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<ProductOptionSubtype>());

        Value = value;
    }

    public static implicit operator string(ProductOptionSubtype value)
        => value.Value;

    public static implicit operator ProductOptionSubtype(string value)
        => new(value);

    public override string ToString()
        => Value;
}
