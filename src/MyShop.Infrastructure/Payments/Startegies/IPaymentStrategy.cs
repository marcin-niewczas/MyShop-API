using MyShop.Core.Models.Orders;
using MyShop.Core.ValueObjects.Orders;
using MyShop.Infrastructure.Payments.Startegies.Models;

namespace MyShop.Infrastructure.Payments.Startegies;
internal interface IPaymentStrategy
{
    PaymentMethod PaymentMethod { get; }

    Task<PaymentStatus> GetPaymentStatusAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default
        );

    Task<CreatedPaymentResponse> CreatePaymentAsync(
        Order order,
        Uri continueUri,
        CancellationToken cancellationToken = default
        );
}
