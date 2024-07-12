using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Dtos.Auth;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Users;
using SharedValueObjects = MyShop.Core.ValueObjects.Shared;
using UserValueObjects = MyShop.Core.ValueObjects.Users;

namespace MyShop.Application.Commands.Account.Users;
public sealed record UpdateRegisteredUserAc(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Gender,
    string? PhoneNumber
    ) : ICommand<ApiResponse<UserDto>>,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        SharedValueObjects.LastName.Validate(LastName, validationMessages);
        SharedValueObjects.FirstName.Validate(FirstName, validationMessages);
        UserValueObjects.DateOfBirth.Validate(DateOfBirth, validationMessages);
        UserPhoneNumber.Validate(PhoneNumber, validationMessages);
        CustomValidators.Enums.MustBeIn<Gender>(
            Gender,
            validationMessages,
            nameof(Gender)
            );
    }
}
