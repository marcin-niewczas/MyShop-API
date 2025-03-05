using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.Products;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ManagementPanel;

namespace MyShop.Application.QueryHandlers.ManagementPanel.Products;
internal sealed class GetPagedProductVariantsByProductIdMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedProductVariantsByProductIdMp, ApiPagedResponse<PagedProductVariantMpDto>>
{
    public async Task<ApiPagedResponse<PagedProductVariantMpDto>> HandleAsync(
        GetPagedProductVariantsByProductIdMp query,
        CancellationToken cancellationToken = default
        )
    {
        var pagedResult = await unitOfWork.ProductVariantRepository.GetPagedProductVariantsMpByProductIdAsync(
            productId: query.Id,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            cancellationToken: cancellationToken
        );

        return new(
            pagedResult.Data.ToPagedProductVariantMpDtos(),
            pagedResult.TotalCount,
            query.PageNumber,
            query.PageSize
            );
    }
}
