using MyShop.Application.Queries.ManagementPanel.Products;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.QueryHandlers.ManagementPanel.Products;
internal sealed class GetProductMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetProductMp, ApiResponse<ProductMpDto>>
{
    public async Task<ApiResponse<ProductMpDto>> HandleAsync(GetProductMp query, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.ProductRepository.GetProductMpAsync(
            id: query.Id,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(Product), query.Id);

        return new(result);
    }
}
