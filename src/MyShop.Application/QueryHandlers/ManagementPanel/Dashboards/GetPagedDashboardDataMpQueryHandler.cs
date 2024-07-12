using MyShop.Application.ApplicationServices.Interfaces;
using MyShop.Application.Dtos.ManagementPanel.Dashboards;
using MyShop.Application.Queries.ManagementPanel.Dashboards;
using MyShop.Application.Responses;

namespace MyShop.Application.QueryHandlers.ManagementPanel.Dashboards;
internal sealed class GetPagedDashboardDataMpQueryHandler(
    IDashboardService dashboardService
    ) : IQueryHandler<GetPagedDashboardDataMp, ApiPagedResponse<BaseDashboardElementMpDto>>
{
    public Task<ApiPagedResponse<BaseDashboardElementMpDto>> HandleAsync(
        GetPagedDashboardDataMp query,
        CancellationToken cancellationToken = default
        ) => dashboardService.GetPagedDashboardDataAsync(query.PageNumber, query.PageSize, cancellationToken);
}
