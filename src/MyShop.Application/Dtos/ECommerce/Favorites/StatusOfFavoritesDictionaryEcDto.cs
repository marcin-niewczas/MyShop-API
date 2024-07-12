using MyShop.Core.Abstractions;
using System.Collections.ObjectModel;

namespace MyShop.Application.Dtos.ECommerce.Favorites;
public sealed class StatusOfFavoritesDictionaryEcDto(
    IDictionary<string, bool> dictionary
    ) : ReadOnlyDictionary<string, bool>(dictionary), IDto;
