using Microsoft.EntityFrameworkCore;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ECommerce;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Core.RepositoryQueryParams.Shared;
using MyShop.Core.ValueObjects.ProductReviews;
using MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories.Utils;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class ProductReviewRespository(
    MainDbContext dbContext
    ) : BaseGenericRepository<ProductReview>(dbContext), IProductReviewRepository
{
    public Task<PagedResult<ProductReview>> GetPagedProductReviewsAsync(
        Guid productId,
        int pageNumber,
        int pageSize,
        GetPagedProductReviewsSortBy? sortBy,
        SortDirection? sortDirection,
        int? productReviewRate,
        CancellationToken cancellationToken = default
        )
    {
        var query = _dbSet
            .Include(i => i.RegisteredUser)
            .ThenInclude(i => i.Photo)
            .Where(e => e.ProductId == productId)
            .Where(e => productReviewRate == null || e.Rate == productReviewRate);

        query = (sortBy, sortDirection) switch
        {
            (GetPagedProductReviewsSortBy.Rate, SortDirection.Ascendant) => query.OrderBy(e => e.Rate),
            (GetPagedProductReviewsSortBy.Rate, SortDirection.Descendant) => query.OrderByDescending(e => e.Rate),
            (GetPagedProductReviewsSortBy.Newest, SortDirection.Ascendant) => query.OrderBy(e => e.CreatedAt).ThenBy(e => e.UpdatedAt),
            _ => query.OrderByDescending(e => e.CreatedAt).ThenByDescending(e => e.UpdatedAt),
        };

        return query.ToPagedResultAsync(pageNumber, pageSize, cancellationToken: cancellationToken);
    }

    public async Task<ProductReviewRateStatEcDto> GetProductReviewRateStatsAsync(
        Guid productId,
        CancellationToken cancellationToken = default
        )
    {
        var baseQuery = _dbSet
            .Where(e => e.ProductId == productId)
            .AsNoTracking();

        var productReviewsCount = await baseQuery
            .CountAsync(cancellationToken);

        var sumProductReviews = productReviewsCount <= 0
            ? 0
            : await baseQuery.SumAsync(e => e.Rate, cancellationToken);


        List<RateCountEcDto> rateCounts;

        if (productReviewsCount <= 0)
        {
            rateCounts = Enumerable.Range(ProductReviewRate.Min, ProductReviewRate.Max).Select(v => new RateCountEcDto
            {
                Rate = v,
                Count = 0
            }).ToList();
        }
        else
        {
            var current = await baseQuery
                 .Where(e => e.ProductId == productId)
                 .GroupBy(e => e.Rate)
                 .Select(e => new RateCountEcDto
                 {
                     Rate = e.Key,
                     Count = e.Count()
                 })
                 .OrderBy(e => e.Rate)
                 .ToListAsync(cancellationToken);

            rateCounts = Enumerable.Range(ProductReviewRate.Min, ProductReviewRate.Max).Select(v => new RateCountEcDto
            {
                Rate = v,
                Count = current.FirstOrDefault(x => x.Rate == v)?.Count ?? 0
            }).ToList();
        }


        return new()
        {
            ProductReviewsCount = productReviewsCount,
            SumProductReviews = sumProductReviews,
            RateCounts = rateCounts
        };
    }
}
