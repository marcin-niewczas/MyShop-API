using MyShop.Application.Abstractions;
using MyShop.Application.Dtos.ECommerce.Favorites;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ECommerce.Favorites;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;

namespace MyShop.Application.QueryHandlers.ECommerce.Favorites;
internal sealed class GetStatusOfFavoritesEcQueryHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetStatusOfFavoritesEc, ApiResponse<StatusOfFavoritesDictionaryEcDto>>
{
    public async Task<ApiResponse<StatusOfFavoritesDictionaryEcDto>> HandleAsync(
        GetStatusOfFavoritesEc query,
        CancellationToken cancellationToken = default
        )
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var favorites = await unitOfWork.FavoriteRepository.GetByPredicateAsync(
            predicate: e => e.RegisteredUserId == userId && query.ProductEncodedNames.Contains(e.EncodedProductVariantName),
            cancellationToken: cancellationToken
            );

        return new(favorites.ToStatusOfFavoritesDictionaryEcDto(query.ProductEncodedNames));
    }
}
