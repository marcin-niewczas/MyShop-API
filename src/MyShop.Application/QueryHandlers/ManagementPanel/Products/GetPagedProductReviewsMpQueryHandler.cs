using MyShop.Application.Dtos.ManagementPanel.ProductReviews;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.Products;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.RepositoryQueryParams.Shared;

namespace MyShop.Application.QueryHandlers.ManagementPanel.Products;
internal sealed class GetPagedProductReviewsMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedProductReviewsByProductIdMp, ApiPagedResponse<ProductReviewMpDto>>
{
    public async Task<ApiPagedResponse<ProductReviewMpDto>> HandleAsync(
        GetPagedProductReviewsByProductIdMp query,
        CancellationToken cancellationToken = default
        )
    {
        var pagedResult = await unitOfWork.ProductReviewRepository.GetPagedProductReviewsAsync(
            productId: query.Id,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            sortBy: TypeMapper.MapOptionalEnum<GetPagedProductReviewsSortBy>(query.SortBy),
            TypeMapper.MapOptionalSortDirection(query.SortDirection),
            productReviewRate: query.ProductReviewRate,
            cancellationToken: cancellationToken
            );

        return new(
            dtos: pagedResult.Data.ToProductReviewMpDtos(),
            totalCount: pagedResult.TotalCount,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize
            );
    }
}
