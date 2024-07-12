using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.Account.Users;
public sealed record UserAddressAcDto : BaseDto
{
    public required string StreetName { get; init; }
    public required string BuildingNumber { get; init; }
    public required string? ApartmentNumber { get; init; }
    public required string City { get; init; }
    public required string ZipCode { get; init; }
    public required string Country { get; init; }
    public required string UserAddressName { get; init; }
    public required bool IsDefault { get; init; }
}
