using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Orders;
using MyShop.Core.Models.Products;
using MyShop.Core.ValueObjects.ShoppingCarts;

namespace MyShop.Core.Models.ShoppingCarts;
public sealed class ShoppingCartItem : BaseTimestampEntity
{
    public ShoppingCartItemQuantity Quantity { get; private set; }
    public ProductVariant ProductVariant { get; private set; } = default!;
    public Guid ProductVariantId { get; private set; }
    public ShoppingCart ShoppingCart { get; private set; } = default!;
    public Guid ShoppingCartId { get; private set; }

    public ShoppingCartItem(Guid productVariantId, Guid shoppingCartId)
    {
        if (productVariantId == default)
        {
            throw new ArgumentException(productVariantId.ToString(), nameof(productVariantId));
        }

        if (shoppingCartId == default)
        {
            throw new ArgumentException(productVariantId.ToString(), nameof(productVariantId));
        }

        ProductVariantId = productVariantId;
        ShoppingCartId = shoppingCartId;
        Quantity = 1;
    }

    public ShoppingCartItemUpdateResult Update(int quantity)
    {
        if (Quantity == quantity)
        {
            return (ShoppingCartItemUpdateState.NoUpdated, this);
        }

        if (ProductVariant is null)
        {
            throw new InvalidOperationException(nameof(ProductVariant));
        }

        if (ShoppingCart is null)
        {
            throw new InvalidOperationException(nameof(ShoppingCart));
        }

        if (ShoppingCart.CheckoutId is not null)
        {
            ShoppingCart.ClearCheckoutId();
        }

        if (quantity >= ShoppingCartItemQuantity.Max)
        {
            if (ProductVariant.Quantity < ShoppingCartItemQuantity.Max)
            {
                Quantity = ProductVariant.Quantity;
                return (ShoppingCartItemUpdateState.UpdatedToMaxQuantityInStock, this);
            }
            else if (quantity > ShoppingCartItemQuantity.Max)
            {
                Quantity = ShoppingCartItemQuantity.Max;
                return (ShoppingCartItemUpdateState.UpdatedToMaxQuantityInShoppingCart, this);
            }
        }

        if (ProductVariant.Quantity < quantity)
        {
            Quantity = ProductVariant.Quantity;
            return (ShoppingCartItemUpdateState.UpdatedToMaxQuantityInStock, this);
        }

        Quantity = quantity;

        return (ShoppingCartItemUpdateState.Updated, this);
    }

    public void Update(ShoppingCartItemQuantity quantity, ProductVariant productVariant)
    {
        if (productVariant is null || productVariant.Id != ProductVariantId)
        {
            throw new ArgumentException(
                $"Parameter {nameof(productVariant)} cannot be null and must have {nameof(productVariant)}.{nameof(productVariant.Id)} equal {nameof(ProductVariantId)}.",
                nameof(productVariant)
                );
        }

        if (quantity < 1)
        {
            throw new ArgumentException(quantity.ToString(), nameof(quantity));
        }

        if (productVariant.Quantity < quantity)
        {
            throw new BadRequestException(
                $"The {nameof(Product)} isn't available with {nameof(Quantity)} equals {quantity}."
                );
        }

        if (ShoppingCart.CheckoutId is not null)
        {
            ShoppingCart.ClearCheckoutId();
        }

        Quantity = quantity;
    }

    public OrderProduct ToOrderProduct(Order order)
    {
        var quantity = ProductVariant.Quantity - Quantity;
        ProductVariant.UpdateQuantity(quantity);

        return new(this, order);
    }
}

public enum ShoppingCartItemUpdateState
{
    NoUpdated = 0,
    UpdatedToMaxQuantityInStock,
    UpdatedToMaxQuantityInShoppingCart,
    Updated,
}
