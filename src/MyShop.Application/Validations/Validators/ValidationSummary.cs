using MyShop.Core.Validations;
using System.Collections.Frozen;

namespace MyShop.Application.Validations.Validators;
public sealed record ValidationSummary
{
    public bool IsValid { get; }
    public IReadOnlyCollection<ValidationMessage> ValidationMessages { get; }
    public FrozenDictionary<string, string[]> GetValidationProblemDictionary()
        => ConvertToValidationProblemDictionary();

    public ValidationSummary(ICollection<ValidationMessage> validationMessages)
    {
        ArgumentNullException.ThrowIfNull(nameof(validationMessages));

        IsValid = validationMessages.Count <= 0;
        ValidationMessages = validationMessages
            .ToList()
            .AsReadOnly();
    }

    private FrozenDictionary<string, string[]> ConvertToValidationProblemDictionary()
         => ValidationMessages
                    .GroupBy(g => g.MemberName)
                    .ToFrozenDictionary(k => k.Key, v => v.SelectMany(m => m.Messages).ToArray());
}
