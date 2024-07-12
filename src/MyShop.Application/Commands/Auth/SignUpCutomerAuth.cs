using MyShop.Application.Validations.Interfaces;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Users;
using SharedValueObjects = MyShop.Core.ValueObjects.Shared;
using UserValueObjects = MyShop.Core.ValueObjects.Users;

namespace MyShop.Application.Commands.Auth;
public sealed record SignUpCutomerAuth(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword,
    DateOnly DateOfBirth,
    string Gender,
    string? PhoneNumber
    ) : ICommand, IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        SharedValueObjects.Email.Validate(Email, validationMessages);
        SharedValueObjects.LastName.Validate(LastName, validationMessages);
        SharedValueObjects.FirstName.Validate(FirstName, validationMessages);
        SharedValueObjects.Password.Validate(Password, validationMessages);
        UserValueObjects.DateOfBirth.Validate(DateOfBirth, validationMessages);

        if (Password != ConfirmPassword)
        {
            validationMessages.Add(new ValidationMessage(nameof(Password), [$"{nameof(Password)} and {nameof(ConfirmPassword)} must the same."]));
        }

        UserValueObjects.DateOfBirth.Validate(DateOfBirth, validationMessages);
        UserPhoneNumber.Validate(PhoneNumber, validationMessages);

        Validations.Validators.CustomValidators.Enums.MustBeIn<Gender>(
            Gender,
            validationMessages,
            nameof(Gender)
            );
    }
}
