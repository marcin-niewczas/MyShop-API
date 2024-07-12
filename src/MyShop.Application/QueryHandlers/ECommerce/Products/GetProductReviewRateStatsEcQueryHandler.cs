using MyShop.Application.Queries.ECommerce.Products;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ECommerce;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.QueryHandlers.ECommerce.Products;
internal sealed class GetProductReviewRateStatsEcQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetProductReviewRateStatsEc, ApiResponse<ProductReviewRateStatEcDto>>
{
    public async Task<ApiResponse<ProductReviewRateStatEcDto>> HandleAsync(
        GetProductReviewRateStatsEc query, 
        CancellationToken cancellationToken = default
        )
    {
        var exist = await unitOfWork.ProductRepository
            .AnyAsync(e => e.Id == query.Id, cancellationToken);

        if (!exist)
            throw new NotFoundException(nameof(Product), query.Id);

        var productReviewRateStats = await unitOfWork.ProductReviewRepository.GetProductReviewRateStatsAsync(
            query.Id,
            cancellationToken
            );

        return new(productReviewRateStats);
    }
}
