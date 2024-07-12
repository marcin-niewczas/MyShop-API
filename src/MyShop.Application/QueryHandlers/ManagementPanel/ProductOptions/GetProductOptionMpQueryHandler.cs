using MyShop.Application.Dtos.ManagementPanel.ProductOptions;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.ProductOptions;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.QueryHandlers.ManagementPanel.ProductOptions;
internal sealed class GetProductOptionMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetProductOptionByIdMp, ApiResponse<ProductOptionMpDto>>
{
    public async Task<ApiResponse<ProductOptionMpDto>> HandleAsync(GetProductOptionByIdMp query, CancellationToken cancellationToken = default)
        => new(
            (await unitOfWork.BaseProductOptionRepository.GetByIdAsync(query.Id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(ProductVariantOption), query.Id)).ToProductOptionMpDto()
            );
}
