using MyShop.Core.Models.ShoppingCarts;

namespace MyShop.Core.HelperModels;
public sealed record ShoppingCartVerifier
{
    public ShoppingCart ShoppingCart { get; private set; }

    public ShoppingCartVerifier(ShoppingCart shoppingCart)
       => ShoppingCart = shoppingCart;


    public IReadOnlyDictionary<Guid, Changed<int, ShoppingCartItem>> Verify()
    {
        var dictionary = new Dictionary<Guid, Changed<int, ShoppingCartItem>>();

        List<ShoppingCartItem> toRemoved = [];

        foreach (var item in ShoppingCart.ShoppingCartItems)
        {
            if (item.ProductVariant.Quantity <= 0)
            {
                toRemoved.Add(item);
                dictionary[item.Id] = new(item.Quantity, 0, item);

                continue;
            }

            if (item.ProductVariant.Quantity < item.Quantity)
            {
                var previousQuantity = item.Quantity;
                item.Update(item.ProductVariant.Quantity);
                dictionary[item.Id] = new(previousQuantity, item.ProductVariant.Quantity, item);

                continue;
            }
        }

        foreach (var item in toRemoved)
        {
            ShoppingCart.RemoveShoppingCartItem(item);
        }

        return dictionary.AsReadOnly();
    }
}
