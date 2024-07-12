using MyShop.Core.Abstractions;
using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.Shared;
using MyShop.Core.ValueObjects.Users;

namespace MyShop.Application.Dtos.ValidatorParameters.Account;
public sealed record UserAddressValidatorParametersAcDto : IDto
{
    public StringValidatorParameters UserAddressNameParams { get; } = new()
    {
        MinLength = UserAddressName.MinLength,
        MaxLength = UserAddressName.MaxLength,
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
    public int MaxCountUserAddresses { get; } = UserAddress.MaxCountUserAddresses;
}
