using MyShop.Application.Dtos.ManagementPanel.ProductVariantPhotoItems;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.ProductVariantPhotoItems;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.RepositoryQueryParams.Commons;

namespace MyShop.Application.QueryHandlers.ManagementPanel.ProductVariantPhotoItems;
internal sealed class GetProductVariantPhotoItemsMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetProductVariantPhotoItemsMp, ApiResponseWithCollection<ProductVariantPhotoItemMpDto>>
{
    public async Task<ApiResponseWithCollection<ProductVariantPhotoItemMpDto>> HandleAsync(
        GetProductVariantPhotoItemsMp query,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.ProductVariantPhotoItemRepository.GetByPredicateAsync(
            predicate: e => e.ProductVariantId.Equals(query.Id),
            includeExpression: i => i.ProductVariantPhoto,
            sortByKeySelector: o => o.Position,
            sortDirection: SortDirection.Ascendant,
            cancellationToken: cancellationToken
            );

        return new(result.ToProductVariantPhotoItemMpDtos());
    }
}
