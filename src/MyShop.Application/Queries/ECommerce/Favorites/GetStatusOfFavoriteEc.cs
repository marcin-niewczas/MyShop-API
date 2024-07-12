using Microsoft.AspNetCore.Mvc;
using MyShop.Application.Dtos;
using MyShop.Application.Responses;

namespace MyShop.Application.Queries.ECommerce.Favorites;
public sealed record GetStatusOfFavoriteEc(
    [FromRoute] string ProductEncodedName
    ) : IQuery<ApiResponse<ValueDto<bool>>>;
