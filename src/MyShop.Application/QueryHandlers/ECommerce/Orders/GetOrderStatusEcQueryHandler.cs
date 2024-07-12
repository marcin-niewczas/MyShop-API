using MyShop.Application.Abstractions;
using MyShop.Application.Dtos.ECommerce.Orders;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ECommerce.Orders;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Orders;
using MyShop.Core.ValueObjects.Users;

namespace MyShop.Application.QueryHandlers.ECommerce.Orders;
internal sealed class GetOrderStatusEcQueryHandler(
     IUserClaimsService userClaimsService,
     IUnitOfWork unitOfWork
    ) : IQueryHandler<GetOrderStatusEc, ApiResponse<OrderStatusEcDto>>
{
    public async Task<ApiResponse<OrderStatusEcDto>> HandleAsync(
        GetOrderStatusEc query, 
        CancellationToken cancellationToken = default
        )
    {
        var userClaims = userClaimsService.GetUserClaimsData();

        var order = await unitOfWork.OrderRepository.GetByIdAsync(
            id: query.Id,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(Order), query.Id);

        if (order.UserId != userClaims.UserId && !UserRole.HasEmployeePermission(userClaims.UserRole))
        {
            throw new ForbiddenException();
        }

        return new(order.ToOrderStatusEcDto());
    }
}
