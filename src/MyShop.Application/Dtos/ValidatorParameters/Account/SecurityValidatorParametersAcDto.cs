using MyShop.Core.Abstractions;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Shared;

namespace MyShop.Application.Dtos.ValidatorParameters.Account;
public sealed record SecurityValidatorParametersAcDto : IDto
{
    public StringValidatorParameters EmailParams { get; } = new()
    {
        MinLength = Email.MinLength,
        MaxLength = Email.MaxLength,
        RegexPattern = CustomRegex.EmailPattern,
    };
    public StringValidatorParameters PasswordParams { get; } = new()
    {
        MinLength = int.Parse(Password.MinLength),
        MaxLength = int.Parse(Password.MaxLength),
        RegexPattern = CustomRegex.PasswordPattern,
        ErrorMessage = Password.GetErrorMessage()
    };
}
