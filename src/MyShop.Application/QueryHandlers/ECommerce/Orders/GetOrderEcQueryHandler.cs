using MyShop.Application.Abstractions;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ECommerce.Orders;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ECommerce;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Orders;

namespace MyShop.Application.QueryHandlers.ECommerce.Orders;
internal sealed class GetOrderEcQueryHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetOrderEc, ApiResponse<OrderWithProductsEcDto>>
{
    public async Task<ApiResponse<OrderWithProductsEcDto>> HandleAsync(
        GetOrderEc query,
        CancellationToken cancellationToken = default
        )
    {
        var userClaims = userClaimsService.GetUserClaimsData();

        var orderDto = await unitOfWork.OrderRepository.GetFullOrderDataEcAsync(
            orderId: query.Id,
            userId: userClaims.UserId,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(Order), query.Id);

        return new(orderDto.ToOrderWithProductsEcDto());
    }
}
