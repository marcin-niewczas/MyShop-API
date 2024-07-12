using MyShop.Core.Abstractions;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Orders;
using MyShop.Core.ValueObjects.Shared;

namespace MyShop.Application.Dtos.ValidatorParameters.ManagementPanel;
public sealed record OrderValidatorParametersMpDto : IDto
{
    public StringValidatorParameters EmailParams { get; } = new()
    {
        MinLength = Email.MinLength,
        MaxLength = Email.MaxLength,
        RegexPattern = CustomRegex.EmailPattern,
    };
    public StringValidatorParameters FirstNameParams { get; } = new()
    {
        MinLength = FirstName.MinLength,
        MaxLength = FirstName.MaxLength,
    };
    public StringValidatorParameters LastNameParams { get; } = new()
    {
        MinLength = LastName.MinLength,
        MaxLength = LastName.MaxLength,
    };
    public StringValidatorParameters PhoneNumberParams { get; } = new()
    {
        MinLength = OrderPhoneNumber.MinLength,
        MaxLength = OrderPhoneNumber.MaxLength,
        RegexPattern = CustomRegex.PhoneNumberPattern,
    };
    public StringValidatorParameters StreetNameParams { get; } = new()
    {
        MinLength = StreetName.MinLength,
        MaxLength = StreetName.MaxLength,
    };
    public StringValidatorParameters BuildingNumberParams { get; } = new()
    {
        MinLength = BuildingNumber.MinLength,
        MaxLength = BuildingNumber.MaxLength,
    };
    public StringValidatorParameters ApartmentNumberParams { get; } = new()
    {
        MinLength = ApartmentNumber.MinLength,
        MaxLength = ApartmentNumber.MaxLength,
        IsRequired = false
    };
    public StringValidatorParameters CityParams { get; } = new()
    {
        MinLength = City.MinLength,
        MaxLength = City.MaxLength,
    };
    public StringValidatorParameters ZipCodeParams { get; } = new()
    {
        MinLength = ZipCode.MinLength,
        MaxLength = ZipCode.MaxLength,
    };
    public StringValidatorParameters CountryParams { get; } = new()
    {
        MinLength = Country.MinLength,
        MaxLength = Country.MaxLength,
    };
    public IReadOnlyCollection<string> AvailableOrderStatusToUpdate { get; }
        = OrderStatus.GetAvailableOrderStatusToUpdate();
}
