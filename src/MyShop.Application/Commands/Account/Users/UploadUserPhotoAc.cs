using Microsoft.AspNetCore.Http;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Dtos.Auth;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Photos;

namespace MyShop.Application.Commands.Account.Users;
public sealed record UploadUserPhotoAc(
    IFormFile UserPhoto
    ) : ICommand<ApiResponse<UserDto>>,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.Photos.Validate(
            UserPhoto,
            PhotoType.UserPhoto,
            validationMessages,
            nameof(UserPhoto)
            );
    }
}
