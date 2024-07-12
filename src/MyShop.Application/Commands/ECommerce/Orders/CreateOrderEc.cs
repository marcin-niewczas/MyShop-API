using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Orders;
using MyShop.Core.ValueObjects.Users;
using SharedValueObjects = MyShop.Core.ValueObjects.Shared;

namespace MyShop.Application.Commands.ECommerce.Orders;
public sealed record CreateOrderEc(
    string CheckoutId,
    string? Email,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string StreetName,
    string BuildingNumber,
    string? ApartmentNumber,
    string ZipCode,
    string City,
    string Country,
    string DeliveryMethod,
    string PaymentMethod
    ) : ICommand<ApiIdResponse>,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        if (string.IsNullOrEmpty(CheckoutId))
        {
            validationMessages.Add(new(
                nameof(CheckoutId),
                [$"The field {nameof(CheckoutId)} is required."]
                ));
        }

        if (Email is not null)
        {
            SharedValueObjects.Email.Validate(Email, validationMessages);
        }

        if (FirstName is not null)
        {
            SharedValueObjects.FirstName.Validate(FirstName, validationMessages);
        }

        if (LastName is not null)
        {
            SharedValueObjects.FirstName.Validate(LastName, validationMessages);
        }

        if (PhoneNumber is not null)
        {
            UserPhoneNumber.Validate(PhoneNumber, validationMessages);
        }

        SharedValueObjects.StreetName.Validate(StreetName, validationMessages);
        SharedValueObjects.BuildingNumber.Validate(BuildingNumber, validationMessages);
        SharedValueObjects.ApartmentNumber.Validate(ApartmentNumber, validationMessages);
        SharedValueObjects.ZipCode.Validate(ZipCode, validationMessages);
        SharedValueObjects.City.Validate(City, validationMessages);
        SharedValueObjects.Country.Validate(Country, validationMessages);

        CustomValidators.Enums.MustBeIn<DeliveryMethod>(
            DeliveryMethod,
            validationMessages,
            nameof(DeliveryMethod)
            );

        CustomValidators.Enums.MustBeIn<PaymentMethod>(
            PaymentMethod,
            validationMessages,
            nameof(PaymentMethod)
            );
    }
}
