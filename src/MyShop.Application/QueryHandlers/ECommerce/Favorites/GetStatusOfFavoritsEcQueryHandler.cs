using MyShop.Application.Abstractions;
using MyShop.Application.Dtos;
using MyShop.Application.Queries.ECommerce.Favorites;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;

namespace MyShop.Application.QueryHandlers.ECommerce.Favorites;
internal sealed class GetStatusOfFavoritsEcQueryHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetStatusOfFavoriteEc, ApiResponse<ValueDto<bool>>>
{
    public async Task<ApiResponse<ValueDto<bool>>> HandleAsync(
        GetStatusOfFavoriteEc query,
        CancellationToken cancellationToken = default
        )
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var isExist = await unitOfWork.FavoriteRepository.AnyAsync(
            predicate: e => e.RegisteredUserId == userId && e.EncodedProductVariantName == query.ProductEncodedName,
            cancellationToken: cancellationToken
            );

        return new(new(isExist));
    }
}
