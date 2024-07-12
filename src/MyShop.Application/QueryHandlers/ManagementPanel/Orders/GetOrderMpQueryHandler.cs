using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.Orders;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Orders;

namespace MyShop.Application.QueryHandlers.ManagementPanel.Orders;
internal sealed class GetOrderMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetOrderMp, ApiResponse<OrderMpDto>>
{
    public async Task<ApiResponse<OrderMpDto>> HandleAsync(
        GetOrderMp query,
        CancellationToken cancellationToken = default
        )
    {
        var entity = await unitOfWork.OrderRepository.GetByIdAsync(
            id: query.Id,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(Order), query.Id);

        return new(entity.ToOrderMpDto());
    }
}
