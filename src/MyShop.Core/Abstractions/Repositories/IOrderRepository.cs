using MyShop.Core.Dtos.Account;
using MyShop.Core.Dtos.ECommerce;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Orders;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Core.RepositoryQueryParams.ECommerce;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;

namespace MyShop.Core.Abstractions.Repositories;
public interface IOrderRepository : IBaseReadRepository<Order>, IBaseWriteRepository<Order>
{
    Task<OrderDetailsMpDto?> GetOrderDetailsMpAsync(
        Guid id,
        CancellationToken cancellationToken = default
        );
    Task<PagedResult<PagedOrderMpDto>> GetPagedOrdersMpAsync(
        int pageNumber,
        int pageSize,
        GetPagedOrdersMpSortBy? sortBy,
        SortDirection? sortDirection,
        DateTimeOffset? fromDate,
        DateTimeOffset? toDate,
        bool inclusiveFromDate = true,
        bool inclusiveToDate = true,
        string? searchPhrase = null,
        CancellationToken cancellationToken = default
       );
    Task<OrderWithProductsEcDto?> GetFullOrderDataEcAsync(
        Guid orderId,
        Guid userId,
        CancellationToken cancellationToken = default
        );
    Task<PagedResult<OrderAcDto>> GetPagedOrdersAcAsync(
        Guid userId,
        int pageNumber,
        int pageSize,
        GetPagedOrdersEcSortBy? sortBy,
        SortDirection? sortDirection,
        DateTimeOffset? fromDate,
        DateTimeOffset? toDate,
        bool inclusiveFromDate = true,
        bool inclusiveToDate = true,
        string? searchPhrase = null,
        CancellationToken cancellationToken = default
        );
}
