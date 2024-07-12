using MyShop.Application.Responses;
using MyShop.Core.Dtos.ManagementPanel;

namespace MyShop.Application.Queries.ManagementPanel.Orders;
public sealed record GetOrderMp(
    Guid Id
    ) : IQuery<ApiResponse<OrderMpDto>>;
