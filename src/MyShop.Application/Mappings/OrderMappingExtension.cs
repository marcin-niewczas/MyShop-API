using MyShop.Application.Dtos.ECommerce.Orders;
using MyShop.Core.Dtos.Auth;
using MyShop.Core.Dtos.ECommerce;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Dtos.Shared;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Orders;
using MyShop.Core.Models.Users;

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

    public static OrderWithProductsEcDto ToOrderWithProductsEcDto(this Order model)
    {
        return new()
        {
            Id = model.Id,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            StreetName = model.StreetName,
            BuildingNumber = model.BuildingNumber,
            ApartmentNumber = model.ApartmentNumber,
            City = model.City,
            ZipCode = model.ZipCode,
            Country = model.Country,
            TotalPrice = model.OrderProducts.Sum(op => op.Quantity * op.Price),
            Status = model.Status,
            DeliveryMethod = model.DeliveryMethod,
            PaymentMethod = model.PaymentMethod,
            RedirectPaymentUri = model.RedirectPaymentUri,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            InvoiceId = model.InvoiceId,
            OrderStatusHistories = model.OrderStatusHistories
                .OrderBy(o => o.CreatedAt)
                .Select(h => new OrderStatusHistoryDto
                {
                    Id = h.Id,
                    CreatedAt = h.CreatedAt,
                    UpdatedAt = h.UpdatedAt,
                    Status = h.Status,
                }).ToArray(),

            OrderProducts = model.OrderProducts.Select(op => new OrderProductEcDto
            {
                Name = string.Concat(
                    op.ProductVariant
                    .Product
                    .ProductDetailOptionValues
                    .First()
                    .Value,
                    " ",
                    op.ProductVariant.Product.Name
                    ),
                CategoryHierarchyName = op.ProductVariant.Product.Category.HierarchyDetail.HierarchyName,
                EncodedName = op.ProductVariant.EncodedName,
                MainPhoto = op.ProductVariant
                                .PhotoItems
                                .Select(p => new PhotoDto(p.ProductVariantPhoto.Uri, p.ProductVariantPhoto.Alt))
                                .FirstOrDefault(),
                OrderId = op.OrderId,
                Price = op.Price,
                Quantity = op.Quantity,
                PriceAll = op.Price * op.Quantity,
                VariantOptionNameValues = op.ProductVariant
                                .Product
                                .ProductProductVariantOptions
                                .Join(op.ProductVariant.ProductVariantOptionValues,
                                      k => k.ProductVariantOptionId,
                                      k => k.ProductOptionId,
                                      (_, v) => new OptionNameValue(v.ProductVariantOption.Name, v.Value))
                                .ToArray()
            }).ToArray()
        };
    }

    public static OrderDetailsMpDto ToOrderDetailsMpDto(this Order model)
    {
        return new()
        {
            Id = model.Id,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            StreetName = model.StreetName,
            BuildingNumber = model.BuildingNumber,
            ApartmentNumber = model.ApartmentNumber,
            City = model.City,
            ZipCode = model.ZipCode,
            Country = model.Country,
            TotalPrice = model.OrderProducts.Sum(op => op.Quantity * op.Price),
            Status = model.Status,
            DeliveryMethod = model.DeliveryMethod,
            PaymentMethod = model.PaymentMethod,
            RedirectPaymentUri = model.RedirectPaymentUri,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            OrderStatusHistories = model.OrderStatusHistories
                .OrderBy(o => o.CreatedAt)
                .Select(h => new OrderStatusHistoryDto
                {
                    Id = h.Id,
                    CreatedAt = h.CreatedAt,
                    UpdatedAt = h.UpdatedAt,
                    Status = h.Status,
                }).ToArray(),
            User = model.User switch
            {
                RegisteredUser ru => new CustomerMeDto
                {
                    Id = ru.Id,
                    FirstName = ru.FirstName,
                    LastName = ru.LastName,
                    PhoneNumber = ru.PhoneNumber,
                    Email = ru.Email,
                    UserRole = ru.Role,
                    Gender = ru.Gender,
                    DateOfBirth = ru.DateOfBirth,
                    PhotoUrl = ru.Photo != null
                            ? ru.Photo!.Uri.ToString()
                            : null
                },
                User u => new UserDto
                {
                    Id = u.Id,
                    UserRole = u.Role
                },
            },
            OrderProducts = model.OrderProducts.Select(op => new OrderProductMpDto
            {
                Id = op.Id,
                Name = string.Concat(
                    op.ProductVariant
                    .Product
                    .ProductDetailOptionValues
                    .First()
                    .Value,
                    " ",
                    op.ProductVariant.Product.Name
                    ),
                ProductVariantId = op.ProductVariantId,
                ProductId = op.ProductVariant.ProductId,
                CategoryHierarchyName = op.ProductVariant.Product.Category.HierarchyDetail.HierarchyName,
                EncodedName = op.ProductVariant.EncodedName,
                MainPhoto = op.ProductVariant
                                .PhotoItems
                                .Select(p => new PhotoDto(p.ProductVariantPhoto.Uri, p.ProductVariantPhoto.Alt))
                                .FirstOrDefault(),
                OrderId = op.OrderId,
                Price = op.Price,
                Quantity = op.Quantity,
                PriceAll = op.Price * op.Quantity,
                VariantOptionNameValues = op.ProductVariant
                                .Product
                                .ProductProductVariantOptions
                                .Join(op.ProductVariant.ProductVariantOptionValues,
                                      k => k.ProductVariantOptionId,
                                      k => k.ProductOptionId,
                                      (_, v) => new OptionNameValue(v.ProductVariantOption.Name, v.Value))
                                .ToArray()
            }).ToArray()
        };
    }
}
