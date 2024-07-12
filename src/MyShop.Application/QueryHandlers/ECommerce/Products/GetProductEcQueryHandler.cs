using MyShop.Application.Queries.ECommerce.Products;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ECommerce;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.QueryHandlers.ECommerce.Products;
internal sealed class GetProductEcQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetProductEc, ApiResponse<BaseProductDetailEcDto>>
{
    public async Task<ApiResponse<BaseProductDetailEcDto>> HandleAsync(
        GetProductEc query,
        CancellationToken cancellationToken = default
        )
    {
        var productDetail = await unitOfWork.ProductVariantRepository.GetProductDetailAsync(query.EncodedName, cancellationToken)
            ?? throw new NotFoundException(nameof(ProductVariant), query.EncodedName);

        return new(productDetail);
    }
}
