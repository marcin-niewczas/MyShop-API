using MyShop.Core.Models.BaseEntities;
using MyShop.Core.ValueObjects.Shared;
using MyShop.Core.ValueObjects.Users;

namespace MyShop.Core.Models.Users;
public sealed class UserAddress : BaseTimestampEntity
{
    public StreetName StreetName { get; private set; } = default!;
    public BuildingNumber BuildingNumber { get; private set; } = default!;
    public ApartmentNumber ApartmentNumber { get; private set; } = default!;
    public City City { get; private set; } = default!;
    public ZipCode ZipCode { get; private set; } = default!;
    public Country Country { get; private set; } = default!;
    public UserAddressName UserAddressName { get; private set; } = default!;
    public bool IsDefault { get; private set; } = false;
    public RegisteredUser RegisteredUser { get; private set; } = default!;
    public Guid RegisteredUserId { get; private set; }

    public const int MaxCountUserAddresses = 5;

    public UserAddress(
        StreetName streetName,
        BuildingNumber buildingNumber,
        ApartmentNumber apartmentNumber,
        City city,
        ZipCode zipCode,
        Country country,
        UserAddressName userAddressName,
        Guid registeredUserId
        )
    {
        ArgumentNullException.ThrowIfNull(streetName, nameof(streetName));
        ArgumentNullException.ThrowIfNull(buildingNumber, nameof(buildingNumber));
        ArgumentNullException.ThrowIfNull(apartmentNumber, nameof(apartmentNumber));
        ArgumentNullException.ThrowIfNull(city, nameof(city));
        ArgumentNullException.ThrowIfNull(zipCode, nameof(zipCode));
        ArgumentNullException.ThrowIfNull(country, nameof(country));
        ArgumentNullException.ThrowIfNull(userAddressName, nameof(userAddressName));

        StreetName = streetName;
        BuildingNumber = buildingNumber;
        ApartmentNumber = apartmentNumber;
        City = city;
        ZipCode = zipCode;
        Country = country;
        UserAddressName = userAddressName;
        RegisteredUserId = registeredUserId;
    }

    private UserAddress() { }

    public UserAddress Update(
        StreetName streetName,
        BuildingNumber buildingNumber,
        ApartmentNumber apartmentNumber,
        City city,
        ZipCode zipCode,
        Country country,
        UserAddressName userAddressName,
        bool? isDefault = null
        )
    {
        ArgumentNullException.ThrowIfNull(streetName, nameof(streetName));
        ArgumentNullException.ThrowIfNull(buildingNumber, nameof(buildingNumber));
        ArgumentNullException.ThrowIfNull(apartmentNumber, nameof(apartmentNumber));
        ArgumentNullException.ThrowIfNull(city, nameof(city));
        ArgumentNullException.ThrowIfNull(zipCode, nameof(zipCode));
        ArgumentNullException.ThrowIfNull(country, nameof(country));
        ArgumentNullException.ThrowIfNull(userAddressName, nameof(userAddressName));

        StreetName = streetName;
        BuildingNumber = buildingNumber;
        ApartmentNumber = apartmentNumber;
        City = city;
        ZipCode = zipCode;
        Country = country;
        UserAddressName = userAddressName;

        if (isDefault is bool value)
        {
            IsDefault = value;
        }

        return this;
    }

    public void SetIsDefault(bool value)
    {
        IsDefault = value;
    }
}
