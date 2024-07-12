using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Core.ValueObjects.MainPageSections;
public sealed record ProductsCarouselSectionType : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues { get; }
        = new string[]
        {
            Bestsellers,
            Newest
        }.AsReadOnly();

    public string Value { get; }

    public const string Newest = nameof(Newest);
    public const string Bestsellers = nameof(Bestsellers);

    public ProductsCarouselSectionType(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<ProductsCarouselSectionType>());

        Value = value;
    }

    public static implicit operator string(ProductsCarouselSectionType value)
        => value.Value;

    public static implicit operator ProductsCarouselSectionType(string value)
        => new(value);

    public override string ToString()
        => Value;
}
