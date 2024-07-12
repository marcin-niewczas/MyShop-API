using Microsoft.EntityFrameworkCore;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Products;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;
using MyShop.Core.RepositoryQueryParams.Shared;
using MyShop.Core.ValueObjects.Categories;
using MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories.Utils;
using System.Linq.Expressions;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class CategoryRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<Category>(dbContext), ICategoryRepository
{
    public Task<Category?> GetByIdAsync(Guid id, GetCategoryMpQueryType categoryQueryType, CancellationToken cancellationToken = default)
        => categoryQueryType switch
        {
            GetCategoryMpQueryType.NoInclude => GetByIdAsync(id, cancellationToken: cancellationToken),
            GetCategoryMpQueryType.IncludeLowerCategories => GetCategoryWithLowerCategoriesByIdAsync(id, cancellationToken),
            _ => throw new ArgumentException(CustomValidators.Enums.GetEnumErrorMessage<GetCategoryMpQueryType>(nameof(categoryQueryType)))
        };

    public async Task<Category?> GetCategoryWithHigherCategoriesByIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        var category = await GetByIdAsync(categoryId, cancellationToken: cancellationToken);

        if (category is null)
            return null;

        if (category.ParentCategoryId is null)
            return category;

        await category.IncludeHigherCategoriesAsync(this, cancellationToken);

        return category;
    }

    public async Task<Category?> GetHigherCategoriesByCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        if (category is null)
            throw new ArgumentNullException(nameof(category), $"Parameter {nameof(category)} cannot be null.");

        if (category.HierarchyDetail is null)
            throw new InvalidOperationException($"Parameter {nameof(category)} cannot have {nameof(Category.HierarchyDetail)} as null.");

        if (category is { HierarchyDetail.Level.Value: 0 } or { ParentCategoryId: null })
            return null;

        var parentCategory = await _dbSet
            .IncludeHigherCategories(category.HierarchyDetail.Level)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == category.ParentCategoryId, cancellationToken)
            ?? throw new InvalidDataInDatabaseException($"{nameof(Category)} with {nameof(IEntity.Id)} equals '{category.Id}' should have {nameof(Category.ParentCategory)}.");

        return parentCategory;

    }

    public async Task<Category?> GetCategoryWithLowerCategoriesByIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        var category = await GetByIdAsync(categoryId, cancellationToken: cancellationToken);

        if (category is null)
            return null;

        await category.IncludeLowerCategoriesAsync(this, cancellationToken);

        return category;
    }

    public async Task<IReadOnlyCollection<Category>> GetLowerCategoriesByCategoryAsync(
        Category category,
        int maxCategoryLevel = CategoryLevel.Max,
        CancellationToken cancellationToken = default
        )
    {
        if (category is null)
            throw new ArgumentNullException(nameof(category), $"Parameter {nameof(category)} cannot be null.");

        if (category.HierarchyDetail is null)
            throw new InvalidOperationException($"Parameter {nameof(category)} cannot have {nameof(Category.HierarchyDetail)} as null.");

        if (category.HierarchyDetail.Level == maxCategoryLevel)
            return [];

        var lowerCategories = await _dbSet
            .IncludeLowerCategories(category.HierarchyDetail.Level + 1, maxCategoryLevel)
            .Where(e => e.ParentCategoryId == category.Id)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return lowerCategories;
    }

    public async Task<PagedResult<Category>?> GetPagedProductCategoriesByCategoryRootIdAsync(
        Guid rootId,
        int pageNumber,
        int pageSize,
        GetPagedCategoriesMpSortBy? sortBy,
        SortDirection? sortDirection,
        string? searchPhrase,
        int maxCategoryLevel = CategoryLevel.Max,
        CancellationToken cancellationToken = default
        )
    {
        var rootCategory = await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == rootId, cancellationToken);

        if (rootCategory is null || rootCategory.ParentCategoryId is not null)
        {
            return null;
        }

        var baseQuery = _dbSet
            .Where(e => (searchPhrase == null || Convert.ToString(e.Name).ToLower().Contains(searchPhrase.ToLower())) && e.HierarchyDetail.RootCategoryId == rootCategory.Id && e.HierarchyDetail.Level == maxCategoryLevel);

        Expression<Func<Category, object?>> sortByExpression = sortBy switch
        {
            GetPagedCategoriesMpSortBy.Name => x => x.Name,
            GetPagedCategoriesMpSortBy.HierarchyName => x => x.HierarchyDetail.HierarchyName,
            GetPagedCategoriesMpSortBy.UpdatedAt => x => x.UpdatedAt,
            _ => x => x.CreatedAt
        };

        baseQuery = sortDirection switch
        {
            SortDirection.Ascendant => baseQuery.OrderBy(sortByExpression),
            _ => baseQuery.OrderByDescending(sortByExpression)
        };

        return await baseQuery
            .ToPagedResultAsync(pageNumber, pageSize, cancellationToken: cancellationToken);
    }

    public Task<PagedResult<Category>> GetPagedDataAsync(
        int pageNumber,
        int pageSize,
        GetPagedCategoriesMpSortBy? sortBy,
        SortDirection? sortDirection,
        string? searchPhrase,
        GetPagedCategoriesQueryType categoriesQueryType,
        int maxCategoryLevel = CategoryLevel.Max,
        CancellationToken cancellationToken = default
        )
    {
        var baseQuery = categoriesQueryType switch
        {
            GetPagedCategoriesQueryType.All => _dbSet,
            GetPagedCategoriesQueryType.Root => _dbSet.Where(e => e.ParentCategoryId == null),
            GetPagedCategoriesQueryType.RootAndLowerCategories => _dbSet.Where(e => e.ParentCategoryId == null).IncludeLowerCategories(0, maxCategoryLevel),
            _ => throw new ArgumentException(CustomValidators.Enums.GetEnumErrorMessage<GetPagedCategoriesQueryType>(nameof(categoriesQueryType))),
        };

        baseQuery = baseQuery
                            .Where(e => searchPhrase == null || Convert.ToString(e.Name).ToLower().Contains(searchPhrase.ToLower()));

        Expression<Func<Category, object?>> sortByExpression = sortBy switch
        {
            GetPagedCategoriesMpSortBy.Name => x => x.Name,
            GetPagedCategoriesMpSortBy.HierarchyName => x => x.HierarchyDetail.HierarchyName,
            GetPagedCategoriesMpSortBy.UpdatedAt => x => x.UpdatedAt,
            _ => x => x.CreatedAt
        };

        baseQuery = sortDirection switch
        {
            SortDirection.Ascendant => baseQuery.OrderBy(sortByExpression),
            _ => baseQuery.OrderByDescending(sortByExpression)
        };

        return baseQuery.ToPagedResultAsync(
            pageNumber: pageNumber,
            pageSize: pageSize,
            asSplitQuery: true,
            cancellationToken: cancellationToken
            );
    }

    public async Task<IReadOnlyCollection<Category>?> GetTheLowestCategoriesByCategoryIdAsync(
        Guid categoryId,
        int maxCategoryLevel = CategoryLevel.Max,
        CancellationToken cancellationToken = default
        )
    {
        var category = await GetByIdAsync(
            id: categoryId,
            cancellationToken: cancellationToken
            );

        return await GetTheLowestCategoriesByCategoryAsync(category, maxCategoryLevel, cancellationToken);
    }

    public async Task<(Category? QueryCategory, IReadOnlyCollection<Category>? TheLowestCategoriesOfQueryCategories)> GetTheLowestCategoriesByEncodedHierarchyNameAsync(
        string encodedHierarchyName,
        int maxCategoryLevel = CategoryLevel.Max,
        CancellationToken cancellationToken = default
        )
    {
        var category = await GetFirstByPredicateAsync(
            predicate: e => e.HierarchyDetail.EncodedHierarchyName.Equals(encodedHierarchyName),
            cancellationToken: cancellationToken
            );

        return (category, await GetTheLowestCategoriesByCategoryAsync(category, maxCategoryLevel, cancellationToken));
    }

    private async Task<IReadOnlyCollection<Category>?> GetTheLowestCategoriesByCategoryAsync(
        Category? category,
        int maxCategoryLevel = CategoryLevel.Max,
        CancellationToken cancellationToken = default
        )
    {
        if (category is null)
            return null;

        if (category.HierarchyDetail is null)
            throw new InvalidDataInDatabaseException($"{nameof(Category)} with {nameof(IEntity.Id)} '{category.Id}' doesn't have a {nameof(category.HierarchyDetail)}.");

        return category switch
        {
            { ParentCategoryId: null, HierarchyDetail.Level.Value: 0 } => await GetByPredicateAsync(
                predicate: e => e.HierarchyDetail.Level == maxCategoryLevel && e.HierarchyDetail.RootCategoryId == category.Id,
                cancellationToken: cancellationToken
                ),
            { ParentCategoryId: not null } when category.HierarchyDetail.Level == maxCategoryLevel => [category],
            { ParentCategoryId: not null, HierarchyDetail.Level.Value: > 0 } when category.HierarchyDetail.Level < maxCategoryLevel => await GetTheLowestCategoriesAsync(
                category: category,
                cancellationToken: cancellationToken
                ),
            _ => throw new InvalidDataInDatabaseException($"Method: {nameof(GetTheLowestCategoriesByCategoryIdAsync)}, {nameof(IEntity.Id)}: {category.Id}"),
        };
    }

    private async Task<IReadOnlyCollection<Category>> GetTheLowestCategoriesAsync(Category category, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(category);

        await category.IncludeLowerCategoriesAsync(this, cancellationToken);

        if (category.ChildCategories is null)
            throw new InvalidOperationException(nameof(category.ChildCategories));

        return GetTheLowestCategoriesRecursive(category).ToList();
    }

    private IEnumerable<Category> GetTheLowestCategoriesRecursive(
        Category category,
        int maxCategoryLevel = CategoryLevel.Max
        )
    {
        if (category is not null &&
            category is { HierarchyDetail: not null } &&
            category.HierarchyDetail.Level == maxCategoryLevel)
            yield return category;

        if (category is not null && category.ChildCategories is not null)
        {
            foreach (var childCategory in category.ChildCategories)
            {
                foreach (var recursiveCategory in GetTheLowestCategoriesRecursive(childCategory))
                {
                    if (recursiveCategory is not null &&
                        recursiveCategory is { ChildCategories: null, HierarchyDetail: not null } &&
                        recursiveCategory.HierarchyDetail.Level == maxCategoryLevel)
                        yield return recursiveCategory;
                }
            }
        }
    }
}
