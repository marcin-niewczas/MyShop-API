using MyShop.Core.Abstractions;

namespace MyShop.Core.Dtos.ManagementPanel;
public sealed record OrderMpDto : BaseTimestampDto
{
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string PhoneNumber { get; init; }
    public required string StreetName { get; init; }
    public required string BuildingNumber { get; init; }
    public required string? ApartmentNumber { get; init; }
    public required string City { get; init; }
    public required string ZipCode { get; init; }
    public required string Country { get; init; }
    public required string Status { get; init; }
    public required string DeliveryMethod { get; init; }
    public required string PaymentMethod { get; init; }
}
