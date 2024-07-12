using MyShop.Application.Dtos.ECommerce.ProductReviews;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ECommerce.Products;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.RepositoryQueryParams.Shared;

namespace MyShop.Application.QueryHandlers.ECommerce.Products;
internal sealed class GetPagedProductReviewsEcQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedProductReviewsEc, ApiPagedResponse<ProductReviewEcDto>>
{
    public async Task<ApiPagedResponse<ProductReviewEcDto>> HandleAsync(
        GetPagedProductReviewsEc query, 
        CancellationToken cancellationToken = default
        )
    {
        var pagedResult = await unitOfWork.ProductReviewRepository.GetPagedProductReviewsAsync(
            productId: query.Id,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            sortBy: TypeMapper.MapOptionalEnum<GetPagedProductReviewsSortBy>(query.SortBy),
            sortDirection: TypeMapper.MapOptionalSortDirection(query.SortDirection),
            productReviewRate: query.ProductReviewRate,
            cancellationToken: cancellationToken
            );

        return new(
            dtos: pagedResult.Data.ToProductReviewEcDtos(),
            totalCount: pagedResult.TotalCount,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize
            );
    }
}
