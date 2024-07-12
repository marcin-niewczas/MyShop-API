using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;

namespace MyShop.Core.ValueObjects.MainPageSections;
public sealed record MainPageSectionPosition : IValidatableValueObject
{
    public int Value { get; }
    public MainPageSectionPosition(int value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator int(MainPageSectionPosition value)
        => value.Value;

    public static implicit operator MainPageSectionPosition(int value)
        => new(value);

    public override string ToString()
        => Value.ToString();

    public const int Min = 0;
    public const int Max = 1;

    public static bool IsValid(int value)
        => value is >= Min and <= Max;

    private static string GetErrorMessage()
        => $"The {nameof(MainPageSectionPosition)} must be inclusive between {Min} and {Max}.";

    private static string GetErrorMessage(int value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is int intValue)
        {
            if (!IsValid(intValue))
            {
                validationMessages.Add(new(nameof(MainPageSectionPosition), [GetErrorMessage(intValue)]));
            }
        }
        else
        {
            validationMessages.Add(
                new(
                    nameof(MainPageSectionPosition),
                    [$"The {nameof(MainPageSectionPosition)} must be a int.", GetErrorMessage()]
                    )
                );
        }
    }
}
