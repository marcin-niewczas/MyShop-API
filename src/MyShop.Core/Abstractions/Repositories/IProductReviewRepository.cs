using MyShop.Core.Dtos.ECommerce;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Core.RepositoryQueryParams.Shared;

namespace MyShop.Core.Abstractions.Repositories;
public interface IProductReviewRepository : IBaseReadRepository<ProductReview>, IBaseWriteRepository<ProductReview>
{
    Task<PagedResult<ProductReview>> GetPagedProductReviewsAsync(
        Guid productId,
        int pageNumber,
        int pageSize,
        GetPagedProductReviewsSortBy? sortBy,
        SortDirection? sortDirection,
        int? productReviewRate,
        CancellationToken cancellationToken = default
        );

    Task<ProductReviewRateStatEcDto> GetProductReviewRateStatsAsync(
        Guid productId,
        CancellationToken cancellationToken = default
        );
}
