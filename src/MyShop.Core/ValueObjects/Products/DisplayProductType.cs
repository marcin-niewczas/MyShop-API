using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Core.ValueObjects.Products;
public sealed record DisplayProductType : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues { get; }
        = new string[]
        {
            MainVariantOption,
            AllVariantOptions,
        }.AsReadOnly();

    public string Value { get; }

    public const string MainVariantOption = "Main Variant Option";
    public const string AllVariantOptions = "All Variant Options";

    public DisplayProductType(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<DisplayProductType>());

        Value = value;
    }

    public static implicit operator string(DisplayProductType value)
        => value.Value;

    public static implicit operator DisplayProductType(string value)
        => new(value);

    public override string ToString()
        => Value;
}
