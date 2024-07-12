using MyShop.Application.Dtos.Auth;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Core.Validations;
using SharedValueObjects = MyShop.Core.ValueObjects.Shared;

namespace MyShop.Application.Commands.Auth;
public sealed record SignInAuth(
    string Email,
    string Password
    ) : ICommand<ApiResponse<AuthDto>>,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        SharedValueObjects.Email.Validate(Email, validationMessages);
        SharedValueObjects.Password.Validate(Password, validationMessages);
    }
}
