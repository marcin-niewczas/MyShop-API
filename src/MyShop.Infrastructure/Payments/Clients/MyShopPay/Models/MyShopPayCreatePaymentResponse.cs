namespace MyShop.Infrastructure.Payments.Clients.MyShopPay.Models;
public sealed record MyShopPayCreatePaymentResponse
{
    public required Guid Id { get; init; }
    public required Uri RedirectUri { get; init; }
}
