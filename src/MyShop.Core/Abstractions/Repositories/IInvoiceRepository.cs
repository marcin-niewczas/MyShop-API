using MyShop.Core.Models.Orders;

namespace MyShop.Core.Abstractions.Repositories;
public interface IInvoiceRepository : IBaseReadRepository<Invoice>, IBaseWriteRepository<Invoice>
{
    Task<int?> GetLastInvoiceNumberAsync(
        DateTimeOffset? now = null,
        CancellationToken cancellationToken = default
        );

    Task<Invoice?> GetInvoiceWithOrderDetailsAsync(
        Guid orderId,
        Guid invoiceId,
        Guid userId,
        CancellationToken cancellationToken = default
        );
}
