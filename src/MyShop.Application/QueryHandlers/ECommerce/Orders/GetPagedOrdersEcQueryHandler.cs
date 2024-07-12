using MyShop.Application.Abstractions;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ECommerce.Orders;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.Account;
using MyShop.Core.RepositoryQueryParams.ECommerce;

namespace MyShop.Application.QueryHandlers.ECommerce.Orders;
internal sealed class GetPagedOrdersEcQueryHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedOrdersEc, ApiPagedResponse<OrderAcDto>>
{
    public async Task<ApiPagedResponse<OrderAcDto>> HandleAsync(
        GetPagedOrdersEc query,
        CancellationToken cancellationToken = default
        )
    {
        var userClaims = userClaimsService.GetUserClaimsData();

        var pagedResult = await unitOfWork.OrderRepository.GetPagedOrdersAcAsync(
            userId: userClaims.UserId,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            sortBy: TypeMapper.MapOptionalEnum<GetPagedOrdersEcSortBy>(query.SortBy),
            sortDirection: TypeMapper.MapOptionalSortDirection(query.SortDirection),
            fromDate: query.FromDate,
            toDate: query.ToDate,
            inclusiveFromDate: query.InclusiveFromDate,
            inclusiveToDate: query.InclusiveToDate,
            searchPhrase: query.SearchPhrase,
            cancellationToken: cancellationToken
            );

        return new(
            dtos: pagedResult.Data,
            totalCount: pagedResult.TotalCount,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize
            );
    }
}
