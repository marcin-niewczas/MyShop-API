using MyShop.Core.Abstractions;
using System.Collections.ObjectModel;

namespace MyShop.Application.Dtos.ECommerce.ShoppingCarts;
public sealed class ShoppingCartIdValueDictionaryEcDto(
    IDictionary<Guid, int> dictionary
    ) : ReadOnlyDictionary<Guid, int>(dictionary),
        IDto;
