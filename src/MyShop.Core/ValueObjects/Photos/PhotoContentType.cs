using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Core.ValueObjects.Photos;
public sealed record PhotoContentType : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues { get; }
        = new string[]
        {
            PNG,
            JPG,
            JPEG,
            WEBP,
        }.AsReadOnly();

    public string Value { get; }

    public const string PNG = "image/png";
    public const string JPG = "image/jpg";
    public const string JPEG = "image/jpeg";
    public const string WEBP = "image/webp";

    public PhotoContentType(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<PhotoContentType>());

        Value = value;
    }

    public static implicit operator string(PhotoContentType value)
        => value.Value;

    public static implicit operator PhotoContentType(string value)
        => new(value);

    public override string ToString()
        => Value;
}
