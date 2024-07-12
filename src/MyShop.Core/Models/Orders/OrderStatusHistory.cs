using MyShop.Core.Models.BaseEntities;
using MyShop.Core.ValueObjects.Orders;

namespace MyShop.Core.Models.Orders;
public sealed class OrderStatusHistory : BaseTimestampEntity
{
    public OrderStatus Status { get; private set; } = default!;
    public Order Order { get; private set; } = default!;
    public Guid OrderId { get; private set; }

    private OrderStatusHistory() { }

    public OrderStatusHistory(OrderStatus status, Order order)
    {
        Status = status;
        OrderId = order.Id;
    }
}
