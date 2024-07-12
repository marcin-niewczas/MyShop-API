using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.Categories;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;
using MyShop.Core.RepositoryQueryParams.Shared;

namespace MyShop.Application.QueryHandlers.ManagementPanel.Categories;
internal sealed class GetPagedCategoriesMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedCategoriesMp, ApiPagedResponse<CategoryMpDto>>
{
    public async Task<ApiPagedResponse<CategoryMpDto>> HandleAsync(
        GetPagedCategoriesMp query,
        CancellationToken cancellationToken = default
        )
    {
        var pagedData = await unitOfWork.CategoryRepository.GetPagedDataAsync(
            query.PageNumber,
            query.PageSize,
            TypeMapper.MapOptionalEnum<GetPagedCategoriesMpSortBy>(query.SortBy),
            TypeMapper.MapOptionalSortDirection(query.SortDirection),
            query.SearchPhrase,
            TypeMapper.MapEnum<GetPagedCategoriesQueryType>(query.QueryType),
            cancellationToken: cancellationToken
            );

        return new(
            pagedData.Data.ToCategoryMpDtos(),
            pagedData.TotalCount,
            query.PageNumber,
            query.PageSize
            );
    }
}
