using MyShop.Application.Dtos.ManagementPanel.Photos;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.Products;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.RepositoryQueryParams.Commons;

namespace MyShop.Application.QueryHandlers.ManagementPanel.Products;
internal sealed class GetPagedProductPhotosMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedProductPhotosMp, ApiPagedResponse<PhotoMpDto>>
{
    public async Task<ApiPagedResponse<PhotoMpDto>> HandleAsync(GetPagedProductPhotosMp query, CancellationToken cancellationToken = default)
    {
        var pagedResult = await unitOfWork.ProductVariantPhotoRepository.GetPagedDataAsync(
            predicate: e => e.ProductVariants.Any(pv => pv.ProductId == query.Id)
                                                  && (query.ExpectProductVariantId == null || !e.ProductVariantPhotoItems.Any(i => i.ProductVariantId == query.ExpectProductVariantId)),
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            sortByKeySelector: e => e.CreatedAt,
            sortDirection: SortDirection.Descendant,
            cancellationToken: cancellationToken
            );

        return new(pagedResult.Data.ToPhotoMpDtos(), pagedResult.TotalCount, query.PageNumber, query.PageSize);
    }
}
