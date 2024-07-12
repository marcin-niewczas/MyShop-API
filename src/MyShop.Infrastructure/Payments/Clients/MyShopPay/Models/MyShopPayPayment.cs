namespace MyShop.Infrastructure.Payments.Clients.MyShopPay.Models;
public sealed record MyShopPayPayment(
    Guid Id,
    decimal Price,
    string RedirectUrl,
    string Status
    );
