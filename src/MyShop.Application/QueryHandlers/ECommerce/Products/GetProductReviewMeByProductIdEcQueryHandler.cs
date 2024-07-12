using MyShop.Application.Abstractions;
using MyShop.Application.Dtos.ECommerce.ProductReviews;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ECommerce.Products;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.QueryHandlers.ECommerce.Products;
internal sealed class GetProductReviewMeByProductIdEcQueryHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetProductReviewMeByProductIdEc, ApiResponse<ProductReviewMeEcDto>>
{
    public async Task<ApiResponse<ProductReviewMeEcDto>> HandleAsync(
        GetProductReviewMeByProductIdEc query,
        CancellationToken cancellationToken = default
        )
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var productReview = await unitOfWork.ProductReviewRepository.GetFirstByPredicateAsync(
            predicate: e => e.ProductId == query.Id && e.RegisteredUserId == userId,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductReview), query.Id);

        return new(productReview.ToProductReviewMeEcDto());
    }
}
