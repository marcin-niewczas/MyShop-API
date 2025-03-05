using MyShop.Core.Models.Orders;

namespace MyShop.Application.Abstractions;
public interface IInvoiceGenerator
{
    MemoryStream Generate(Invoice invoice);
}
