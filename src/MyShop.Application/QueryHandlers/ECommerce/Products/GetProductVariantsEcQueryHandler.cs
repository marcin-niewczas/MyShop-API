using MyShop.Application.Queries.ECommerce.Products;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ECommerce;

namespace MyShop.Application.QueryHandlers.ECommerce.Products;
internal sealed class GetProductVariantsEcQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetProductVariantsEc, ApiResponseWithCollection<ProductVariantEcDto>>
{
    public async Task<ApiResponseWithCollection<ProductVariantEcDto>> HandleAsync(
        GetProductVariantsEc query,
        CancellationToken cancellationToken = default
        ) => new(await unitOfWork.ProductVariantRepository.GetProductVariantsByEncodedNameAsync(query.EncodedName, cancellationToken));
}
