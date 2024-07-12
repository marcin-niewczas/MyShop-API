using MyShop.Application.Validations.Interfaces;
using MyShop.Core.Validations;
using SharedValueObjects = MyShop.Core.ValueObjects.Shared;

namespace MyShop.Application.Commands.Account.Users;
public sealed record UpdateRegisteredUserEmailAc(
    string NewEmail,
    string Password
    ) : ICommand,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        SharedValueObjects.Email.Validate(NewEmail, validationMessages);
        SharedValueObjects.Password.Validate(Password, validationMessages);
    }
}