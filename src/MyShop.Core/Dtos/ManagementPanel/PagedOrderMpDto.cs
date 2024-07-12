using MyShop.Core.Abstractions;
using MyShop.Core.Models.BaseEntities;

namespace MyShop.Core.Dtos.ManagementPanel;
public sealed record PagedOrderMpDto : BaseTimestampDto, IModel
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
    public required decimal TotalPrice { get; init; }
    public required string Status { get; init; }
    public required string DeliveryMethod { get; init; }
    public required string PaymentMethod { get; init; }
}
