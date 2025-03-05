using MyShop.Core.Models.BaseEntities;

namespace MyShop.Core.Models.Orders;
public sealed class Invoice : BaseTimestampEntity
{
    public int InvoiceNumber { get; private set; } = default!;
    public Order Order { get; private set; } = default!;

    public Invoice(
        Order order,
        int invoiceNumber
        )
    {
        InvoiceNumber = invoiceNumber;
        Order = order;
    }

    private Invoice() { }

    public string GetInvoiceFormatedNumber()
        => $"INV/{CreatedAt.Month}/{CreatedAt.Year}-{InvoiceNumber}";
}
