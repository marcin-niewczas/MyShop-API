using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedValueObjects = MyShop.Core.ValueObjects.Shared;

namespace MyShop.Application.Commands.ManagementPanel.Orders;
public sealed record UpdateOrderMp(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string StreetName,
    string BuildingNumber,
    string ApartmentNumber,
    string ZipCode,
    string City,
    string Country,
    string OrderStatus
    ) : ICommand, IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        SharedValueObjects.Email.Validate(Email, validationMessages);
        SharedValueObjects.FirstName.Validate(FirstName, validationMessages);
        SharedValueObjects.LastName.Validate(LastName, validationMessages);
        OrderPhoneNumber.Validate(PhoneNumber, validationMessages);
        SharedValueObjects.StreetName.Validate(StreetName, validationMessages);
        SharedValueObjects.BuildingNumber.Validate(BuildingNumber, validationMessages);
        SharedValueObjects.ApartmentNumber.Validate(ApartmentNumber, validationMessages);
        SharedValueObjects.ZipCode.Validate(ZipCode, validationMessages);
        SharedValueObjects.City.Validate(City, validationMessages);
        SharedValueObjects.Country.Validate(Country, validationMessages);
        CustomValidators.Enums.MustBeIn<OrderStatus>(
            OrderStatus,
            validationMessages,
            nameof(OrderStatus)
            );
    }
}
