using MyShop.Application.Dtos.ECommerce.Orders;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Models.Orders;

namespace MyShop.Application.Mappings;
internal static class OrderMappingExtension
{
    public static OrderStatusEcDto ToOrderStatusEcDto(this Order model)
        => new()
        {
            Id = model.Id,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            Status = model.Status,
            RedirectPaymentUri = model.RedirectPaymentUri,
        };

    public static OrderMpDto ToOrderMpDto(this Order model)
        => new()
        {
            Id = model.Id,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            Status = model.Status,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            StreetName = model.StreetName,
            BuildingNumber = model.BuildingNumber,
            ApartmentNumber = model.ApartmentNumber,
            ZipCode = model.ZipCode,
            City = model.City,
            Country = model.Country,
            DeliveryMethod = model.DeliveryMethod,
            PaymentMethod = model.PaymentMethod,
        };
}
