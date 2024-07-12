using MyShop.Infrastructure.Payments.Clients.MyShopPay.Models;

namespace MyShop.Infrastructure.Payments.Clients.MyShopPay;
internal interface IMyShopPayHttpClient
{
    Task<MyShopPayCreatePaymentResponse> CreatePaymentAsync(
        MyShopPayCreatePaymentRequestModel model,
        CancellationToken cancellationToken = default
        );
    Task<MyShopPayPayment> GetPaymentAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default
        );
}
