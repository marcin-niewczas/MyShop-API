using MyShop.Application.Dtos.ManagementPanel.ProductOptions;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.ProductOptions;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;

namespace MyShop.Application.QueryHandlers.ManagementPanel.ProductOptions;
internal sealed class GetPagedProductOptionsMpQueryHandler(
    IUnitOfWork _unitOfWork
        ) : IQueryHandler<GetPagedProductOptionsMp, ApiPagedResponse<ProductOptionMpDto>>
{
    public async Task<ApiPagedResponse<ProductOptionMpDto>> HandleAsync(GetPagedProductOptionsMp query, CancellationToken cancellationToken = default)
    {
        var pagedResult = await _unitOfWork.BaseProductOptionRepository.GetPagedDataAsync(
            query.PageNumber,
            query.PageSize,
            TypeMapper.MapEnum<ProductOptionTypeMpQueryType>(query.QueryType),
            TypeMapper.MapEnum<ProductOptionSubtypeMpQueryType>(query.SubqueryType),
            TypeMapper.MapOptionalEnum<GetPagedProductOptionsMpSortBy>(query.SortBy),
            TypeMapper.MapOptionalSortDirection(query.SortDirection),
            query.SearchPhrase,
            cancellationToken
            );

        return new(
               pagedResult.Data.ToProductOptionMpDtos(),
               pagedResult.TotalCount,
               query.PageNumber,
               query.PageSize
               );
    }
}
