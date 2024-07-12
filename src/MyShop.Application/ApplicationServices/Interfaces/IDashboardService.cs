using MyShop.Application.Dtos.ManagementPanel.Dashboards;
using MyShop.Application.Responses;

namespace MyShop.Application.ApplicationServices.Interfaces;
internal interface IDashboardService
{
    Task<ApiPagedResponse<BaseDashboardElementMpDto>> GetPagedDashboardDataAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default
        );
}
