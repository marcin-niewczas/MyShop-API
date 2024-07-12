using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.Orders;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;

namespace MyShop.Application.QueryHandlers.ManagementPanel.Orders;
internal sealed class GetPagedOrdersMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedOrdersMp, ApiPagedResponse<PagedOrderMpDto>>
{
    public async Task<ApiPagedResponse<PagedOrderMpDto>> HandleAsync(
        GetPagedOrdersMp query,
        CancellationToken cancellationToken = default
        )
    {
        var pagedResult = await unitOfWork.OrderRepository.GetPagedOrdersMpAsync(
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            sortBy: TypeMapper.MapOptionalEnum<GetPagedOrdersMpSortBy>(query.SortBy),
            sortDirection: TypeMapper.MapOptionalEnum<SortDirection>(query.SortDirection),
            fromDate: query.FromDate,
            toDate: query.ToDate,
            inclusiveFromDate: query.InclusiveFromDate,
            inclusiveToDate: query.InclusiveToDate,
            searchPhrase: query.SearchPhrase,
            cancellationToken: cancellationToken
            );

        return new(
            dtos: pagedResult.Data,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            totalCount: pagedResult.TotalCount
            );
    }
}
