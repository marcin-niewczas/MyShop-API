using MyShop.Application.Validations.Interfaces;
using MyShop.Core.Validations;
using SharedValueObjects = MyShop.Core.ValueObjects.Shared;

namespace MyShop.Application.Commands.Account.Users;
public sealed record UpdateRegisteredUserPasswordAc(
    string Password,
    string NewPassword,
    string ConfirmNewPassword,
    bool LogoutOtherDevices
    ) : ICommand, IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {

        SharedValueObjects.Password.Validate(Password, validationMessages);
        SharedValueObjects.Password.Validate(NewPassword, validationMessages);

        if (NewPassword != ConfirmNewPassword)
        {
            validationMessages.Add(new(
                nameof(NewPassword), 
                [$"{nameof(NewPassword)} and {nameof(ConfirmNewPassword)} must the same."]
                ));
        }
    }
}
