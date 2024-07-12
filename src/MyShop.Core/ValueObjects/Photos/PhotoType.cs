using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Core.ValueObjects.Photos;
public sealed record PhotoType : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues { get; }
        = new string[]
        {
            ProductVariantPhoto,
            UserPhoto,
            WebsiteHeroPhoto,
        }.AsReadOnly();

    public string Value { get; }

    public const string ProductVariantPhoto = "Product Variant Photo";
    public const string UserPhoto = "User Photo";
    public const string WebsiteHeroPhoto = "Website Hero Photo";

    public PhotoType(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<PhotoType>());

        Value = value;
    }

    public static implicit operator string(PhotoType value)
        => value.Value;

    public static implicit operator PhotoType(string value)
        => new(value);

    public override string ToString()
        => Value;
}
