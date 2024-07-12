using MyShop.Core.Models.Orders;
using MyShop.Core.ValueObjects.Orders;
using MyShop.Infrastructure.Payments.Clients.MyShopPay;
using MyShop.Infrastructure.Payments.Startegies.Models;

namespace MyShop.Infrastructure.Payments.Startegies;
internal sealed class MyShopPayPaymentStrategy(
    IMyShopPayHttpClient client
    ) : IPaymentStrategy
{
    public PaymentMethod PaymentMethod => PaymentMethod.MyShopPay;

    public async Task<CreatedPaymentResponse> CreatePaymentAsync(
        Order order,
        Uri continueUri,
        CancellationToken cancellationToken = default
        )
    {
        var sumOrderProducts = order.OrderProducts
            .Select(e => e.Price * e.Quantity)
            .Sum();

        var response = await client.CreatePaymentAsync(new(sumOrderProducts, continueUri), cancellationToken);

        return new(response.Id, response.RedirectUri);
    }

    public async Task<PaymentStatus> GetPaymentStatusAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default
        )
    {
        var response = await client.GetPaymentAsync(paymentId, cancellationToken);

        return MapStatus(response.Status);
    }

    private static PaymentStatus MapStatus(string status)
        => status switch
        {
            "Paid" => PaymentStatus.Paid,
            "WaitingForPayment" => PaymentStatus.InProgress,
            "Failed" => PaymentStatus.Failed,
            _ => PaymentStatus.Unknown,
        };
}
