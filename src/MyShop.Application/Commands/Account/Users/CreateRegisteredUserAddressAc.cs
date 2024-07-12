using MyShop.Application.Dtos.Account.Users;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Core.Validations;
using SharedValueObjects = MyShop.Core.ValueObjects.Shared;
using UserValueObjects = MyShop.Core.ValueObjects.Users;

namespace MyShop.Application.Commands.Account.Users;
public sealed record CreateRegisteredUserAddressAc(
     string StreetName,
     string BuildingNumber,
     string? ApartmentNumber,
     string City,
     string ZipCode,
     string Country,
     string UserAddressName,
     bool IsDefault
    ) : ICommand<ApiResponse<UserAddressAcDto>>,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        SharedValueObjects.StreetName.Validate(StreetName, validationMessages);
        SharedValueObjects.BuildingNumber.Validate(BuildingNumber, validationMessages);
        SharedValueObjects.ApartmentNumber.Validate(ApartmentNumber, validationMessages);
        SharedValueObjects.ZipCode.Validate(ZipCode, validationMessages);
        SharedValueObjects.City.Validate(City, validationMessages);
        SharedValueObjects.Country.Validate(Country, validationMessages);
        UserValueObjects.UserAddressName.Validate(UserAddressName, validationMessages);
    }
}
