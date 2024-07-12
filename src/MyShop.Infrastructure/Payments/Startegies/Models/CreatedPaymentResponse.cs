namespace MyShop.Infrastructure.Payments.Startegies.Models;
internal sealed record CreatedPaymentResponse(
    Guid Id,
    Uri RedirectUri
    );
