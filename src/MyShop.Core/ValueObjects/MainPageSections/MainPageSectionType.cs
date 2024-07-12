using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Core.ValueObjects.MainPageSections;
public sealed record MainPageSectionType : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues { get; }
        = new string[]
        {
            WebsiteProductsCarouselSection,
            WebsiteHeroSection,
        }.AsReadOnly();

    public string Value { get; }

    public const string WebsiteProductsCarouselSection = "Website Products Carousel Section";
    public const string WebsiteHeroSection = "Website Hero Section";

    public MainPageSectionType(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<MainPageSectionType>());

        Value = value;
    }

    public static implicit operator string(MainPageSectionType value)
        => value.Value;

    public static implicit operator MainPageSectionType(string value)
        => new(value);

    public override string ToString()
        => Value;
}
