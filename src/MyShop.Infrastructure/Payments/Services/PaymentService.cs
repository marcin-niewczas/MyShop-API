using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Orders;
using MyShop.Infrastructure.Options;
using MyShop.Infrastructure.Payments.Exceptions;
using MyShop.Infrastructure.Payments.Startegies;
using MyShop.Infrastructure.Payments.Startegies.Models;

namespace MyShop.Infrastructure.Payments.Services;
internal sealed class PaymentService(
    IEnumerable<IPaymentStrategy> paymentStrategies,
    IOptions<WebSPAClientOptions> options,
    ILogger<PaymentService> logger,
    IUnitOfWork unitOfWork
    ) : IPaymentService
{
    public async Task<CreatedPaymentResponse?> CreatePaymentAsync(
        Order order,
        CancellationToken cancellationToken = default
        )
    {
        var startegy = paymentStrategies
            .FirstOrDefault(s => s.PaymentMethod == order.PaymentMethod)
            ?? throw new NotImplementedException($"Not implemented {order.PaymentMethod} strategy.");

        var continueUri = new Uri(options.Value.CurrentUri, $"orders/{order.Id}/summaries");

        try
        {
            return await startegy.CreatePaymentAsync(order, continueUri, cancellationToken);
        }
        catch (PaymentClientException exception)
        {
            logger.LogError("Exception: {Type} | Response: {Message}", typeof(PaymentClientException), exception.Message);
            return null;
        };
    }

    public async Task<(bool IsUpdated, Order Order)> UpdatePaymentOrderStatusAsync(
        Order order,
        CancellationToken cancellationToken = default
        )
    {
        var startegy = paymentStrategies
            .FirstOrDefault(s => s.PaymentMethod == order.PaymentMethod);

        if (startegy is null)
        {
            logger.LogError("Not implemented {PaymentMethod} strategy.", order.PaymentMethod);

            return (false, order);
        }

        if (order.PaymentId is Guid guid)
        {
            try
            {
                var status = await startegy.GetPaymentStatusAsync(guid, cancellationToken);

                switch (status)
                {
                    case PaymentStatus.Paid:
                        order.UpdateAsPaid(async (orderStatusHistory) => await unitOfWork.AddAsync(orderStatusHistory));
                        break;
                    case PaymentStatus.Failed:
                        order.UpdateAsPaymentFailed(async (orderStatusHistory) => await unitOfWork.AddAsync(orderStatusHistory));
                        break;
                    default:
                        return (false, order);
                }

                return (true, order);
            }
            catch (PaymentClientException exception)
            {
                logger.LogError("Exception: {Type} | Response: {Message}", typeof(PaymentClientException), exception.Message);
                return (false, order);
            };
        }

        return (false, order);
    }
}
