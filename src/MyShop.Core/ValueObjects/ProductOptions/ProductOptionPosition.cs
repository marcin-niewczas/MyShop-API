using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;

namespace MyShop.Core.ValueObjects.ProductOptions;
public sealed record ProductOptionPosition : IValidatableValueObject
{
    public int Value { get; }
    public ProductOptionPosition(int value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), GetErrorMessage());
        }

        Value = value;
    }

    public static implicit operator int(ProductOptionPosition value)
        => value.Value;

    public static implicit operator ProductOptionPosition(int value)
        => new(value);

    public const int Min = 0;

    public static bool IsValid(int value)
        => value is >= Min;

    public override string ToString()
        => Value.ToString();

    private static string GetErrorMessage()
        => $"The {nameof(ProductOptionPosition)} must be greater or equal to {Min}.";

    private static string GetErrorMessage(int value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is int intValue)
        {
            if (!IsValid(intValue))
            {
                validationMessages.Add(new(nameof(ProductOptionPosition), [GetErrorMessage(intValue)]));
            }
        }
        else
        {
            validationMessages.Add(
                new(
                    nameof(ProductOptionPosition),
                    [$"The {nameof(ProductOptionPosition)} must be an int.", GetErrorMessage()]
                    )
                );
        }
    }
}
