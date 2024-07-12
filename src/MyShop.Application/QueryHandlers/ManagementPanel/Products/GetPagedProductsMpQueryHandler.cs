using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.Products;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;

namespace MyShop.Application.QueryHandlers.ManagementPanel.Products;
internal sealed class GetPagedProductsMpQueryHandler(
    IUnitOfWork unitOfWork
        ) : IQueryHandler<GetPagedProductsMp, ApiPagedResponse<PagedProductMpDto>>
{
    public async Task<ApiPagedResponse<PagedProductMpDto>> HandleAsync(GetPagedProductsMp query, CancellationToken cancellationToken)
    {
        var pagedResult = await unitOfWork.ProductRepository.GetPagedProductsMpAsync(
            query.PageNumber,
            query.PageSize,
            TypeMapper.MapOptionalEnum<GetPagedProductsMpSortBy>(query.SortBy),
            TypeMapper.MapOptionalSortDirection(query.SortDirection),
            query.SearchPhrase,
            cancellationToken
            );

        return new(
               pagedResult.Data,
               pagedResult.TotalCount,
               query.PageNumber,
               query.PageSize
               );
    }
}
