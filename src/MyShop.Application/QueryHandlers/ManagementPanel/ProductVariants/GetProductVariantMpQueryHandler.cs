using MyShop.Application.Queries.ManagementPanel.ProductVariants;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.QueryHandlers.ManagementPanel.ProductVariants;
internal sealed class GetProductVariantMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetProductVariantMp, ApiResponse<ProductVariantMpDto>>
{
    public async Task<ApiResponse<ProductVariantMpDto>> HandleAsync(
        GetProductVariantMp query, 
        CancellationToken cancellationToken = default
        )
    {
        var model = await unitOfWork.ProductVariantRepository.GetProductVariantMpAsync(
            query.Id,
            cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductVariant), query.Id);

        return new(model);
    }
}
