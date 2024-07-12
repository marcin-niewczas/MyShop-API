using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.ECommerce.Orders;
public sealed record OrderStatusEcDto : BaseTimestampDto
{
    public required string Status { get; init; }
    public required Uri? RedirectPaymentUri { get; init; }
}
