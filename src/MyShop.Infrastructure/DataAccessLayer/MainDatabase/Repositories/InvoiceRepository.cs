using Microsoft.EntityFrameworkCore;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Orders;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class InvoiceRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<Invoice>(dbContext), IInvoiceRepository
{
    public async Task<int?> GetLastInvoiceNumberAsync(
        DateTimeOffset? now = null,
        CancellationToken cancellationToken = default
        )
    {
        now ??= DateTimeOffset.UtcNow;

        var result = await _dbSet
            .Where(e => e.CreatedAt.Month == now.Value.Month && e.CreatedAt.Year == now.Value.Year)
            .OrderByDescending(o => o.InvoiceNumber)
            .FirstOrDefaultAsync(cancellationToken);

        return result?.InvoiceNumber;
    }

    public Task<Invoice?> GetInvoiceWithOrderDetailsAsync(
        Guid orderId,
        Guid invoiceId,
        Guid userId,
        CancellationToken cancellationToken = default
        )
    {
        return _dbSet
            .Include(i => i.Order)
            .ThenInclude(i => i.OrderProducts.OrderBy(o => o.Price))
            .ThenInclude(i => i.ProductVariant)
            .ThenInclude(i => i.Product)
            .ThenInclude(i => i.ProductDetailOptionValues
                                .Where(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main)
                                .Take(1))
            .ThenInclude(i => i.ProductDetailOption)
            .Include(i => i.Order)
            .ThenInclude(i => i.OrderProducts)
            .ThenInclude(i => i.ProductVariant)
            .ThenInclude(i => i.Product)
            .ThenInclude(i => i.ProductProductVariantOptions.OrderBy(o => o.Position))
            .Include(i => i.Order)
            .ThenInclude(i => i.OrderProducts)
            .ThenInclude(i => i.ProductVariant)
            .ThenInclude(i => i.ProductVariantOptionValues)
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(e => e.Id == invoiceId && e.Order.Id == orderId && e.Order.UserId == userId, cancellationToken);
    }
}
