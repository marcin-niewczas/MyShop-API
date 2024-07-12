using MyShop.Application.Dtos.ECommerce.Orders;
using MyShop.Application.Responses;

namespace MyShop.Application.Queries.ECommerce.Orders;
public sealed record GetOrderStatusEc(Guid Id)
    : IQuery<ApiResponse<OrderStatusEcDto>>;
