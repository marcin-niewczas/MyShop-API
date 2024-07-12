using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.ValidatorParameters.Account;
public sealed record UserPhotoValidatorParametersAcDto : IDto
{
    public PhotoValidatorParameters PhotoFileParams { get; }
        = new PhotoValidatorParameters();
}
