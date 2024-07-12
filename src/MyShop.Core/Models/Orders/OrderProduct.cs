using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Products;
using MyShop.Core.Models.ShoppingCarts;
using MyShop.Core.ValueObjects.Orders;

namespace MyShop.Core.Models.Orders;
public sealed class OrderProduct : BaseTimestampEntity
{
    public OrderProductQuantity Quantity { get; private set; } = default!;
    public OrderProductPrice Price { get; private set; } = default!;
    public ProductVariant ProductVariant { get; private set; } = default!;
    public Guid ProductVariantId { get; private set; }
    public Order Order { get; private set; } = default!;
    public Guid OrderId { get; private set; }

    private OrderProduct() { }

    public OrderProduct(
        ShoppingCartItem shoppingCartItem,
        Order order
        )
    {
        Quantity = shoppingCartItem.Quantity;
        Price = shoppingCartItem.ProductVariant.Price;
        ProductVariant = shoppingCartItem.ProductVariant;
        ProductVariantId = shoppingCartItem.ProductVariantId;
        Order = order;
        OrderId = order.Id;
    }
}
