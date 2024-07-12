using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Core.ValueObjects.ProductOptions;
public sealed record ProductOptionSortType : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues { get; }
        = new string[]
        {
            Alphabetically,
            Custom
        }.AsReadOnly();

    public string Value { get; }

    public const string Alphabetically = nameof(Alphabetically);
    public const string Custom = nameof(Custom);

    public ProductOptionSortType(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<ProductOptionSortType>());

        Value = value;
    }

    public static implicit operator string(ProductOptionSortType value)
        => value.Value;

    public static implicit operator ProductOptionSortType(string value)
        => new(value);

    public override string ToString()
        => Value;
}
