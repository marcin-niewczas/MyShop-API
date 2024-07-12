using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;
using MyShop.Core.RepositoryQueryParams.Shared;
using MyShop.Core.ValueObjects.Categories;

namespace MyShop.Core.Abstractions.Repositories;
public interface ICategoryRepository : IBaseReadRepository<Category>, IBaseWriteRepository<Category>
{
    Task<Category?> GetByIdAsync(
        Guid id,
        GetCategoryMpQueryType categoryQueryType,
        CancellationToken cancellationToken = default
        );
    Task<PagedResult<Category>> GetPagedDataAsync(
        int pageNumber,
        int pageSize,
        GetPagedCategoriesMpSortBy? sortBy,
        SortDirection? sortDirection,
        string? searchPhrase,
        GetPagedCategoriesQueryType categoriesQueryType,
        int maxCategoryLevel = CategoryLevel.Max,
        CancellationToken cancellationToken = default
        );
    Task<PagedResult<Category>?> GetPagedProductCategoriesByCategoryRootIdAsync(
        Guid rootId,
        int pageNumber,
        int pageSize,
        GetPagedCategoriesMpSortBy? sortBy,
        SortDirection? sortDirection,
        string? searchPhrase,
        int maxCategoryLevel = CategoryLevel.Max,
        CancellationToken cancellationToken = default
        );
    Task<Category?> GetCategoryWithHigherCategoriesByIdAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default
        );
    Task<Category?> GetHigherCategoriesByCategoryAsync(
        Category category,
        CancellationToken cancellationToken = default
        );
    Task<IReadOnlyCollection<Category>> GetLowerCategoriesByCategoryAsync(
        Category category,
        int maxCategoryLevel = CategoryLevel.Max,
        CancellationToken cancellationToken = default
        );
    Task<IReadOnlyCollection<Category>?> GetTheLowestCategoriesByCategoryIdAsync(
        Guid categoryId,
        int maxCategoryLevel = CategoryLevel.Max,
        CancellationToken cancellationToken = default
        );
    Task<(Category? QueryCategory, IReadOnlyCollection<Category>? TheLowestCategoriesOfQueryCategories)> GetTheLowestCategoriesByEncodedHierarchyNameAsync(
        string encodedHierarchyName,
        int maxCategoryLevel = CategoryLevel.Max,
        CancellationToken cancellationToken = default
        );
}
