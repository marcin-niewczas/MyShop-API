using Microsoft.EntityFrameworkCore;
using MyShop.Application.Dtos.ManagementPanel.Products;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.Products;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.RepositoryQueryParams.Commons;

namespace MyShop.Application.QueryHandlers.ManagementPanel.Products;
internal sealed class GetPagedProductDetailOptionsByProductIdMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedProductDetailOptionsByProductIdMp, ApiPagedResponse<ProductDetailOptionOfProductMpDto>>
{
    public async Task<ApiPagedResponse<ProductDetailOptionOfProductMpDto>> HandleAsync(
        GetPagedProductDetailOptionsByProductIdMp query,
        CancellationToken cancellationToken = default
        )
    {
        var pagedResult = await unitOfWork.ProductProductDetailOptionValueRespository.GetPagedDataAsync(
            predicate: e => e.ProductId == query.Id,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            sortByKeySelector: o => o.Position,
            sortDirection: SortDirection.Ascendant,
            include: i => i.Include(e => e.ProductDetailOptionValue).ThenInclude(e => e.ProductDetailOption),
            cancellationToken: cancellationToken
            );

        return new(
            pagedResult.Data.ToProductDetailOptionOfProductMpDtos(),
            pagedResult.TotalCount,
            query.PageNumber,
            query.PageSize
            );
    }
}
