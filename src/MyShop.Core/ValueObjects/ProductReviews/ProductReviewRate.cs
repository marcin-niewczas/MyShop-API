using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;

namespace MyShop.Core.ValueObjects.ProductReviews;
public sealed record ProductReviewRate : IValidatableValueObject
{
    public int Value { get; }

    public ProductReviewRate(int value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException(GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator int(ProductReviewRate value)
        => value.Value;

    public static implicit operator ProductReviewRate(int value)
        => new(value);

    public override string ToString()
        => Value.ToString();

    public const int Min = 1;
    public const int Max = 6;

    private static bool IsValid(int value)
        => value is >= Min and <= Max;

    private static string GetErrorMessage()
        => $"The {nameof(ProductReviewRate)} must be inclusive between {Min} and {Max}.";

    private static string GetErrorMessage(int value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is not null)
        {
            if (value is int intValue)
            {
                if (!IsValid(intValue))
                {
                    validationMessages.Add(new(nameof(ProductReviewRate), [GetErrorMessage(intValue)]));
                }
            }
            else
            {
                validationMessages.Add(
                    new(
                        nameof(ProductReviewRate),
                        [$"The {nameof(ProductReviewRate)} must be a int.", GetErrorMessage()]
                        )
                    );
            }
        }
    }
}
