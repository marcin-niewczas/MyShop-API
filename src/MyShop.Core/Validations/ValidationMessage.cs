using MyShop.Core.Utils;

namespace MyShop.Core.Validations;
public sealed record ValidationMessage
{
    public string MemberName { get; }
    public IReadOnlyCollection<string> Messages { get; }

    public ValidationMessage(string memberName, IEnumerable<string> messages)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(memberName, nameof(memberName));

        if (messages.IsNullOrEmpty() && messages.Any(m => m is null))
        {
            throw new ArgumentException(
                $"Parameter {nameof(messages)} must have greater than 0 elements and every value must be not null."
                );
        }

        MemberName = memberName;
        Messages = messages
                    .ToArray()
                    .AsReadOnly();
    }
}
