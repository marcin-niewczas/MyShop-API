using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ValueObjects.ProductReviews;
public sealed record ProductReviewText : IValidatableValueObject
{
    public string? Value { get; }
    public ProductReviewText(string? value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException(GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator string?(ProductReviewText value)
        => value?.Value;

    public static implicit operator ProductReviewText(string? value)
        => new(value);

    public override string? ToString()
        => Value;

    public const int MinLength = 1;
    public const int MaxLength = 1000;

    private static bool IsValid([NotNullWhen(false)] string? value)
        => value is null || (value.Trim().Length is not 0 && value.Length is >= MinLength and <= MaxLength);

    private static string GetErrorMessage()
        => $"The {nameof(ProductReviewText)} must be between {MinLength} and {MaxLength} and not be whitespace.";

    private static string GetErrorMessage(string value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is not null)
        {
            if (value is string stringValue)
            {
                if (!IsValid(stringValue))
                {
                    validationMessages.Add(new(nameof(ProductReviewText), [GetErrorMessage(stringValue)]));
                }
            }
            else
            {
                validationMessages.Add(
                    new(
                        nameof(ProductReviewText),
                        [$"The {nameof(ProductReviewText)} must be a string.", GetErrorMessage()]
                        )
                    );
            }
        }
    }
}
