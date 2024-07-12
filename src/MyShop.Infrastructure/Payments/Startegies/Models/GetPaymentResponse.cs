namespace MyShop.Infrastructure.Payments.Startegies.Models;
internal record GetPaymentResponse(
    Guid PaymentId,
    PaymentStatus PaymentStatus
    );
