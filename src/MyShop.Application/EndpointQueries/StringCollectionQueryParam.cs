using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace MyShop.Application.EndpointQueries;
public sealed class StringCollectionQueryParam(
    IList<string> list
    ) : ReadOnlyCollection<string>(list), IParsable<StringCollectionQueryParam>
{
    public static string GetSwaggerDescription() => "Example: [Value1,Value2,Value3]";

    public static StringCollectionQueryParam Parse(
        string s,
        IFormatProvider? provider
        )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(s);

        if (!s.StartsWith('[') || !s.EndsWith(']'))
        {
            throw new ArgumentException("Value must start with '[' and end with ']'.");
        }

        return new(s[1..^1].Split(','));
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out StringCollectionQueryParam result
        )
    {
        result = default;

        if (string.IsNullOrWhiteSpace(s) || !s.StartsWith('[') || !s.EndsWith(']'))
        {
            return false;
        }

        result = new(s[1..^1].Split(','));

        return true;
    }
}
