using MyShop.Core.Abstractions;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Shared;
using MyShop.Core.ValueObjects.Users;

namespace MyShop.Application.Dtos.ValidatorParameters.Auth;
public sealed record AuthValidatorParametersDto : IDto
{
    public StringValidatorParameters EmailParams { get; } = new()
    {
        MinLength = Email.MinLength,
        MaxLength = Email.MaxLength,
        RegexPattern = CustomRegex.EmailPattern,
    };
    public StringValidatorParameters PasswordParams { get; } = new()
    {
        RegexPattern = CustomRegex.PasswordPattern,
        ErrorMessage = Password.GetErrorMessage()
    };
    public StringValidatorParameters FirstNameParams { get; } = new()
    {
        MinLength = FirstName.MinLength,
        MaxLength = FirstName.MaxLength,
    };
    public StringValidatorParameters LastNameParams { get; } = new()
    {
        MinLength = LastName.MinLength,
        MaxLength = LastName.MaxLength,
    };
    public StringValidatorParameters PhoneNumberParams { get; } = new()
    {
        MinLength = UserPhoneNumber.MinLength,
        MaxLength = UserPhoneNumber.MaxLength,
        RegexPattern = CustomRegex.PhoneNumberPattern,
    };
    public DateOnlyValidatorParameters DateOfBirthParams { get; } = new()
    {
        Min = DateOfBirth.Min,
        Max = DateOfBirth.Max,
    };
    public IReadOnlyCollection<string> GenderValues { get; }
        = Gender.AllowedValues.OfType<string>().ToArray();
}
