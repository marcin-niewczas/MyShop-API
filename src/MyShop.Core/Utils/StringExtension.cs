using Humanizer;

namespace MyShop.Core.Utils;
public static class StringExtension
{
    public static string ToCamelCase(this string input)
        => input.Camelize();

    public static string ToCategoryTitleCase(this string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        var splitInput = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        if (splitInput.Length == 0)
            return input;

        return string.Join(" ", splitInput.Select(s => s.Length > 1 ? char.ToUpper(s[0]) + s[1..].ToLower() : char.ToUpper(s[0]).ToString()));
    }

    public static string ToTitleCase(this string input)
        => input.Titleize();

    public static string ToKebabCase(this string input)
        => input.Kebaberize();

    public static string ToPluralize(this string input, bool inputIsKnownToBeSingular = true)
       => input.Pluralize(inputIsKnownToBeSingular);

    public static string ToPluralizeTitleCase(this string input, bool inputIsKnownToBeSingular = true)
       => input.ToTitleCase().ToPluralize(inputIsKnownToBeSingular);

    public static string ToTrimmedString(this string input)
        => string.Join(" ", input.Split(Array.Empty<string>(), StringSplitOptions.RemoveEmptyEntries));

    public static string ToEncodedName(this string input)
        => input.ToKebabCase();

    public static string ReplaceFirst(this string input, string oldValue, string newValue)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);
        ArgumentException.ThrowIfNullOrWhiteSpace(oldValue);
        ArgumentException.ThrowIfNullOrWhiteSpace(newValue);

        var position = input.IndexOf(oldValue);

        if (position < 0)
            return input;

        return string.Concat(input.AsSpan(0, position), newValue, input.AsSpan(position + oldValue.Length));
    }

    public static string ReplaceLast(this string input, string oldValue, string newValue)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);
        ArgumentException.ThrowIfNullOrWhiteSpace(oldValue);
        ArgumentException.ThrowIfNullOrWhiteSpace(newValue);

        var position = input.LastIndexOf(oldValue);

        if (position < 0)
            return input;

        return string.Concat(input.AsSpan(0, position), newValue, input.AsSpan(position + oldValue.Length));
    }
}
