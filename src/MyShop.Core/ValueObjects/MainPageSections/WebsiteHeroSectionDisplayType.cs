using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Core.ValueObjects.MainPageSections;
public sealed record WebsiteHeroSectionDisplayType : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues { get; }
        = new string[]
        {
            Carousel,
            Grid
        }.AsReadOnly();

    public string Value { get; }

    public const string Carousel = nameof(Carousel);
    public const string Grid = nameof(Grid);

    public WebsiteHeroSectionDisplayType(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<WebsiteHeroSectionDisplayType>());

        Value = value;
    }

    public static implicit operator string(WebsiteHeroSectionDisplayType value)
        => value.Value;

    public static implicit operator WebsiteHeroSectionDisplayType(string value)
        => new(value);

    public override string ToString()
        => Value;
}
