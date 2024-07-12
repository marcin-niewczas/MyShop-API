using MyShop.Application.Responses;
using MyShop.Core.Dtos.ManagementPanel;

namespace MyShop.Application.Queries.ManagementPanel.Orders;
public sealed record GetOrderDetailsMp(
    Guid Id
    ) : IQuery<ApiResponse<OrderDetailsMpDto>>;
