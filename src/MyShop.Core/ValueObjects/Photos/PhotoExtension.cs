using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Core.ValueObjects.Photos;
public sealed record PhotoExtension : IAllowedValues
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

    public const string PNG = ".png";
    public const string JPG = ".jpg";
    public const string JPEG = ".jpeg";
    public const string WEBP = ".webp";

    public PhotoExtension(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<PhotoExtension>());

        Value = value;
    }

    public static implicit operator string(PhotoExtension value)
        => value.Value;

    public static implicit operator PhotoExtension(string value)
        => new(value);

    public override string ToString()
        => Value;
}
