using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Products;
using MyShop.Core.Models.Users;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.ShoppingCarts;

namespace MyShop.Core.Models.ShoppingCarts;
public sealed class ShoppingCart : BaseTimestampEntity
{
    public string? CheckoutId { get; private set; }
    public User User { get; private set; } = default!;
    public Guid UserId { get; private set; }
    public IReadOnlyCollection<ShoppingCartItem> ShoppingCartItems => _shoppingCartItems;
    private readonly List<ShoppingCartItem> _shoppingCartItems = default!;

    public const int MaxShoppingCartItems = 10;

    private ShoppingCart() { }

    public ShoppingCart(Guid userId)
    {
        if (userId == default)
        {
            throw new ArgumentException($"Paramater {nameof(userId)} cannot be default.", nameof(userId));
        }

        UserId = userId;
        _shoppingCartItems = [];
    }

    public (ShoppingCartItem shoppingCartItem, bool isUpdated) AddShoppingCartItem(ProductVariant productVariant)
    {
        ArgumentNullException.ThrowIfNull(_shoppingCartItems);
        ArgumentNullException.ThrowIfNull(productVariant);

        var item = _shoppingCartItems
            .FirstOrDefault(e => e.ProductVariantId == productVariant.Id);

        if (item is null)
        {
            if (_shoppingCartItems.Count >= MaxShoppingCartItems)
            {
                throw new BadRequestException(
                    $"The {nameof(ShoppingCart).ToTitleCase} can only have max. {MaxShoppingCartItems} items."
                    );
            }

            if (productVariant.Quantity <= 0)
            {
                throw new BadRequestException(
                    $"The {nameof(Product)} isn't available."
                    );
            }

            item = new ShoppingCartItem(productVariant.Id, Id);

            _shoppingCartItems.Add(item);

            if (CheckoutId is not null)
            {
                ClearCheckoutId();
            }

            return (item, false);
        }

        if (item.Quantity >= ShoppingCartItemQuantity.Max)
        {
            throw new BadRequestException(
                $"You can add max. {ShoppingCartItemQuantity.Max} pieces of this product."
                );
        }

        var incrementQuantity = item.Quantity + 1;

        if (productVariant.Quantity < incrementQuantity)
        {
            throw new BadRequestException(
                $"The {nameof(Product)} isn't available with {nameof(ShoppingCartItem.Quantity)} equals {incrementQuantity}."
                );
        }

        item.Update(incrementQuantity, productVariant);

        return (item, true);
    }

    public ShoppingCartItemUpdateResult UpdateShoppingCartItem(Guid shoppingCartItemId, int quantity)
    {
        ArgumentNullException.ThrowIfNull(_shoppingCartItems);

        var item = _shoppingCartItems
            .FirstOrDefault(e => e.Id == shoppingCartItemId)
            ?? throw new BadRequestException($"Invalid {nameof(shoppingCartItemId)}/{nameof(shoppingCartItemId)}s.");

        return item.Update(quantity);
    }

    public bool RemoveShoppingCartItem(Guid shoppingCartItemId)
    {
        ArgumentNullException.ThrowIfNull(_shoppingCartItems);

        var item = _shoppingCartItems
            .FirstOrDefault(e => e.Id == shoppingCartItemId);

        if (item is null)
        {
            return false;
        }

        var result = _shoppingCartItems.Remove(item);

        if (result && CheckoutId is not null)
        {
            ClearCheckoutId();
        }

        return result;
    }

    public bool RemoveShoppingCartItem(ShoppingCartItem shoppingCartItem)
    {
        ArgumentNullException.ThrowIfNull(_shoppingCartItems);
        ArgumentNullException.ThrowIfNull(shoppingCartItem);

        if (shoppingCartItem.ShoppingCartId != Id)
        {
            throw new ArgumentException($"Incorrect {nameof(ShoppingCartItem)} to execute {nameof(RemoveShoppingCartItem)} method.");
        }

        return _shoppingCartItems.Remove(shoppingCartItem);
    }

    public void SetCheckoutId()
    {
        if (_shoppingCartItems is null)
        {
            throw new InvalidOperationException(
                $"{nameof(ShoppingCartItems)} must be included, when you want execute '{nameof(SetCheckoutId)}' method."
                );
        }

        if (_shoppingCartItems.Count <= 0)
        {
            throw new BadRequestException(
                $"The {nameof(ShoppingCart)} cannot be empty for checkout."
                );
        }

        CheckoutId = $"{DateTimeOffset.UtcNow:fffffff}-{Guid.NewGuid()}";
    }

    public void ClearCheckoutId()
    {
        CheckoutId = null;
    }

    public void ClearShoppingCart()
    {
        if (_shoppingCartItems is null)
        {
            throw new InvalidOperationException($"{nameof(ShoppingCartItems)} must be included for execute {nameof(ClearShoppingCart)} method.");
        }

        _shoppingCartItems.Clear();
    }
}
