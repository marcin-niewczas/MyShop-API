namespace MyShop.Application.Queries.ECommerce.Orders;
public sealed record GetInvoiceEc(
    Guid Id,
    Guid InvoiceId
    ) : IQuery<MemoryStream>;

