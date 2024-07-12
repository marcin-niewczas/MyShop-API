using MyShop.Application.Dtos.ECommerce.Categories;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ECommerce.Categories;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;
using MyShop.Core.RepositoryQueryParams.Shared;

namespace MyShop.Application.QueryHandlers.ECommerce.Categories;
internal sealed class GetPagedCategoriesEcQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedCategoriesEc, ApiPagedResponse<CategoryEcDto>>
{
    public async Task<ApiPagedResponse<CategoryEcDto>> HandleAsync(
        GetPagedCategoriesEc query, 
        CancellationToken cancellationToken = default
        )
    {
        var pagedData = await unitOfWork.CategoryRepository.GetPagedDataAsync(
             pageNumber: query.PageNumber,
             pageSize: query.PageSize,
             searchPhrase: query.SearchPhrase,
             categoriesQueryType: TypeMapper.MapEnum<GetPagedCategoriesQueryType>(query.QueryType),
             sortBy: GetPagedCategoriesMpSortBy.Name,
             sortDirection: SortDirection.Ascendant,
             cancellationToken: cancellationToken
             );

        return new(
            dtos: pagedData.Data.ToCategoryEcDtos()!,
            totalCount: pagedData.TotalCount,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize
            );
    }
}
