namespace MyShop.Core.ValueObjects.Categories;
public sealed record CategoryLevel
{
    public const int Max = 2;

    public int Value { get; }
    public CategoryLevel(int value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException(GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator int(CategoryLevel value)
        => value.Value;

    public static implicit operator CategoryLevel(int value)
        => new(value);

    public static bool IsValid(int value)
        => value is >= 0 and <= Max;

    public override string ToString()
        => Value.ToString();

    private static string GetErrorMessage()
        => $"The {nameof(CategoryLevel)} must be inclusive between 0 and {Max}.";

    private static string GetErrorMessage(int value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";
}
