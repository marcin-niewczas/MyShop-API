using MyShop.Core.Models.ShoppingCarts;

namespace MyShop.Core.HelperModels;
public sealed record ShoppingCartVerifier
{
    public required ShoppingCart ShoppingCart { get; init; }
    public IReadOnlyCollection<ShoppingCartItemDetail> ShoppingCartItemToVerifies => _shoppingCartItemToVerifies;
    public List<ShoppingCartItemDetail> _shoppingCartItemToVerifies;

    public ShoppingCartVerifier(List<ShoppingCartItemDetail> shoppingCartItemToVerifies)
       => _shoppingCartItemToVerifies = shoppingCartItemToVerifies;


    public IReadOnlyDictionary<Guid, Changed<int, ShoppingCartItemDetail>> Verify()
    {
        var dictionary = new Dictionary<Guid, Changed<int, ShoppingCartItemDetail>>();

        List<ShoppingCartItemDetail> toRemoved = [];

        foreach (var item in _shoppingCartItemToVerifies)
        {
            if (item.ProductVariant.Quantity <= 0)
            {
                toRemoved.Add(item);
                dictionary[item.ShoppingCartItem.Id] = new(item.ShoppingCartItem.Quantity, 0, item);

                continue;
            }

            if (item.ProductVariant.Quantity < item.ShoppingCartItem.Quantity)
            {
                var previousQuantity = item.ShoppingCartItem.Quantity;
                item.ShoppingCartItem.Update(item.ProductVariant.Quantity);
                dictionary[item.ShoppingCartItem.Id] = new(previousQuantity, item.ProductVariant.Quantity, item);

                continue;
            }
        }

        foreach (var item in toRemoved)
        {
            _shoppingCartItemToVerifies.Remove(item);
            ShoppingCart.RemoveShoppingCartItem(item.ShoppingCartItem);
        }

        return dictionary.AsReadOnly();
    }
}
