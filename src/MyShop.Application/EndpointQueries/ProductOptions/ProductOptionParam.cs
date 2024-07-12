using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace MyShop.Application.EndpointQueries.ProductOptions;
public sealed class ProductOptionParam(
    IDictionary<string, string[]> dictionary
    ) : ReadOnlyDictionary<string, string[]>(dictionary),
        IParsable<ProductOptionParam>
{
    public static string GetSwaggerDescription() => "Example: [Key1:Value1,Value2;Key2:Value1,Value2]";

    public static ProductOptionParam Parse(
        string s,
        IFormatProvider? provider
        )
    {
        if (string.IsNullOrWhiteSpace(s) || s[0] != '[' || s[^1] != ']')
        {
            throw new ArgumentException(s, nameof(s));
        }

        var splittedString = s.TrimStart('[').TrimEnd(']').Split(';');

        string[] tempKeyValues;
        string tempKey;
        string[] tempValues;

        var dictionary = new Dictionary<string, string[]>();

        foreach (var element in splittedString)
        {
            if (string.IsNullOrEmpty(element))
            {
                throw new ArgumentException(s, nameof(s));
            }

            tempKeyValues = element.Split(':');

            if (tempKeyValues.Length is not 2)
            {
                throw new ArgumentException(s, nameof(s));
            }

            tempKey = tempKeyValues[0];

            if (string.IsNullOrWhiteSpace(tempKey))
            {
                throw new ArgumentException(s, nameof(s));
            }

            tempValues = tempKeyValues[1].Split(",");

            if (tempValues.Length <= 0 || tempValues.Any(string.IsNullOrWhiteSpace) || !dictionary.TryAdd(tempKey, tempValues))
            {
                throw new ArgumentException(s, nameof(s));
            }

        }

        return new(dictionary);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ProductOptionParam result
        )
    {
        if (string.IsNullOrWhiteSpace(s) || s[0] != '[' || s[^1] != ']')
        {
            result = null;
            return false;
        }

        var splittedString = s.TrimStart('[').TrimEnd(']').Split(';');

        string[] tempKeyValues;
        string tempKey;
        string[] tempValues;

        var dictionary = new Dictionary<string, string[]>();

        foreach (var element in splittedString)
        {
            if (string.IsNullOrEmpty(element))
            {
                result = null;
                return false;
            }

            tempKeyValues = element.Split(':');

            if (tempKeyValues.Length is not 2)
            {
                result = null;
                return false;
            }

            tempKey = tempKeyValues[0];

            if (string.IsNullOrWhiteSpace(tempKey))
            {
                result = null;
                return false;
            }

            tempValues = tempKeyValues[1].Split(',');

            if (tempValues.Length <= 0 || tempValues.Any(string.IsNullOrWhiteSpace) || !dictionary.TryAdd(tempKey, tempValues))
            {
                result = null;
                return false;
            }
        }

        result = new(dictionary);
        return true;
    }
}
