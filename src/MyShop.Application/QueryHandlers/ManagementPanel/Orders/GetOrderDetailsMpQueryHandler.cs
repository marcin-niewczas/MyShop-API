using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.Orders;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Orders;

namespace MyShop.Application.QueryHandlers.ManagementPanel.Orders;
internal sealed class GetOrderDetailsMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetOrderDetailsMp, ApiResponse<OrderDetailsMpDto>>
{
    public async Task<ApiResponse<OrderDetailsMpDto>> HandleAsync(
        GetOrderDetailsMp query,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.OrderRepository.GetOrderDetailsMpAsync(
            query.Id,
            cancellationToken
            ) ?? throw new NotFoundException(nameof(Order), query.Id);

        return new(result.ToOrderDetailsMpDto());
    }
}
