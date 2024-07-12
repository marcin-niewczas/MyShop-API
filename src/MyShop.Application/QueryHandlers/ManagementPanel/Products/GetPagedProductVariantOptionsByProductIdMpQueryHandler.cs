using Microsoft.EntityFrameworkCore;
using MyShop.Application.Dtos.ManagementPanel.Products;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.Products;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.RepositoryQueryParams.Commons;

namespace MyShop.Application.QueryHandlers.ManagementPanel.Products;
public sealed class GetPagedProductVariantOptionsByProductIdMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedProductVariantOptionsByProductIdMp, ApiPagedResponse<ProductVariantOptionOfProductMpDto>>
{
    public async Task<ApiPagedResponse<ProductVariantOptionOfProductMpDto>> HandleAsync(GetPagedProductVariantOptionsByProductIdMp query, CancellationToken cancellationToken = default)
    {
        var pagedResult = await unitOfWork.ProductProductVariantOptionRespository.GetPagedDataAsync(
            predicate: e => e.Product.Id == query.Id && (query.SearchPhrase == null || Convert.ToString(e.Product.Name).ToLower().Contains(query.SearchPhrase.ToLower())),
            include: i => i.Include(e => e.ProductVariantOption),
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            sortByKeySelector: e => e.Position,
            sortDirection: SortDirection.Ascendant,
            cancellationToken: cancellationToken
            );

        return new(
            pagedResult.Data.ToProductVariantOptionOfProductMpDtos(),
            pagedResult.TotalCount,
            query.PageNumber,
            query.PageSize
            );
    }


}
