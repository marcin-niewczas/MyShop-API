using MyShop.Application.Responses;
using MyShop.Core.Dtos.ECommerce;

namespace MyShop.Application.Queries.ECommerce.Orders;
public sealed record GetOrderEc(Guid Id)
    : IQuery<ApiResponse<OrderWithProductsEcDto>>;
