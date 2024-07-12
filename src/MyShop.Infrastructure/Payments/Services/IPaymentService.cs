using MyShop.Core.Models.Orders;
using MyShop.Infrastructure.Payments.Startegies.Models;

namespace MyShop.Infrastructure.Payments.Services;
internal interface IPaymentService
{
    Task<CreatedPaymentResponse?> CreatePaymentAsync(
        Order order,
        CancellationToken cancellationToken = default
        );

    Task<(bool IsUpdated, Order Order)> UpdatePaymentOrderStatusAsync(
        Order order,
        CancellationToken cancellationToken = default
        );
}
