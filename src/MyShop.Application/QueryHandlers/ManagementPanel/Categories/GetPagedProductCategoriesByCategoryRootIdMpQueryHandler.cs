using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.Categories;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;

namespace MyShop.Application.QueryHandlers.ManagementPanel.Categories;
internal sealed class GetPagedProductCategoriesByCategoryRootIdMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedProductCategoriesByCategoryRootIdMp, ApiPagedResponse<CategoryMpDto>>
{
    public async Task<ApiPagedResponse<CategoryMpDto>> HandleAsync(
        GetPagedProductCategoriesByCategoryRootIdMp query, 
        CancellationToken cancellationToken = default
        )
    {
        var pagedResult = await unitOfWork.CategoryRepository.GetPagedProductCategoriesByCategoryRootIdAsync(
            query.RootId,
            query.PageNumber,
            query.PageSize,
            TypeMapper.MapOptionalEnum<GetPagedCategoriesMpSortBy>(query.SortBy),
            TypeMapper.MapOptionalSortDirection(query.SortDirection),
            query.SearchPhrase,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(
                $"{nameof(Category)} with {nameof(GetPagedProductCategoriesByCategoryRootIdMp.RootId)} {query.RootId} not found."
                );

        return new(
            pagedResult.Data.ToCategoryMpDtos(),
            pagedResult.TotalCount,
            query.PageNumber,
            query.PageSize
            );
    }
}
