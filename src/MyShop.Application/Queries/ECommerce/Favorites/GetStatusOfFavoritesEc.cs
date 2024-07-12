using Microsoft.AspNetCore.Mvc;
using MyShop.Application.Dtos.ECommerce.Favorites;
using MyShop.Application.EndpointQueries;
using MyShop.Application.Responses;

namespace MyShop.Application.Queries.ECommerce.Favorites;
public sealed record GetStatusOfFavoritesEc(
    [FromQuery] StringCollectionQueryParam ProductEncodedNames
    ) : IQuery<ApiResponse<StatusOfFavoritesDictionaryEcDto>>;
