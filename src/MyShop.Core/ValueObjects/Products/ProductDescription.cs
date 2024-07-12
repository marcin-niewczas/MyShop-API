using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace MyShop.Core.ValueObjects.Products;
public sealed record ProductDescription : IValidatableValueObject
{
    public string? Value { get; }
    public ProductDescription(string? value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException(GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator string?(ProductDescription value)
        => value?.Value;

    public static implicit operator ProductDescription(string? value)
        => new(value);

    public override string? ToString()
        => Value;

    public const int MinLength = 1;
    public const int MaxLength = 3000;

    private static bool IsValid([NotNullWhen(false)] string? value)
        => value is null || (value.Trim().Length is not 0 && value.Length is >= MinLength and <= MaxLength);

    private static string GetErrorMessage()
        => $"The {nameof(ProductDescription)} must be between {MinLength} and {MaxLength} and not be whitespace.";

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
                    validationMessages.Add(new(nameof(ProductDescription), [GetErrorMessage(stringValue)]));
                }
            }
            else
            {
                validationMessages.Add(
                    new(
                        nameof(ProductDescription),
                        [$"The {nameof(ProductDescription)} must be a string.", GetErrorMessage()]
                        )
                    );
            }
        }
    }
}
