using Microsoft.EntityFrameworkCore;
using MyShop.Application.Validations.Validators;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Products;
using MyShop.Core.RepositoryQueryParams.Commons;
using System.Linq.Expressions;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories.Utils;
internal static class QueryableExtension
{
    public async static Task<PagedResult<TModel>> ToPagedResultAsync<TModel>(
        this IQueryable<TModel> query,
        int pageNumber,
        int pageSize,
        bool asSplitQuery = false,
        CancellationToken cancellationToken = default
        ) where TModel : class, IModel
    {
        query = query.AsNoTracking();

        var pagedQuery = query
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize);

        if (asSplitQuery)
        {
            pagedQuery = pagedQuery.AsSplitQuery();
        }

        return new PagedResult<TModel>(
            Data: await pagedQuery.ToListAsync(cancellationToken),
            TotalCount: await query.CountAsync(cancellationToken)
            );
    }

    public static IOrderedQueryable<TEntity> ApplySort<TEntity>(
        this IQueryable<TEntity> query,
        Expression<Func<TEntity, object?>>? sortByKeySelector,
        SortDirection? sortDirection)
        where TEntity : class, IEntity
    {
        if (sortByKeySelector is not null && sortDirection is not null)
        {
            return sortDirection switch
            {
                SortDirection.Ascendant => query.OrderBy(sortByKeySelector),
                SortDirection.Descendant => query.OrderByDescending(sortByKeySelector),
                _ => throw new ArgumentException(CustomValidators.SortParams.SortDirection.ErrorMessage(nameof(sortDirection)))
            };
        }
        else if (query is IQueryable<ITimestampEntity>)
        {
            return query.OrderByDescending(e => ((ITimestampEntity)e).CreatedAt);
        }

        return query.OrderByDescending(e => e.Id);
    }

    public static IQueryable<TEntity> ApplyThenSort<TEntity>(
        this IOrderedQueryable<TEntity> query,
        Expression<Func<TEntity, object?>>? sortByKeySelector,
        SortDirection? sortDirection)
        where TEntity : class, IEntity
    {
        if (sortByKeySelector is not null && sortDirection is not null)
        {
            return sortDirection switch
            {
                SortDirection.Ascendant => query.ThenBy(sortByKeySelector),
                SortDirection.Descendant => query.ThenByDescending(sortByKeySelector),
                _ => throw new ArgumentException(CustomValidators.SortParams.SortDirection.ErrorMessage(nameof(sortDirection)))
            };
        }
        else if (query is IQueryable<ITimestampEntity>)
        {
            return query.ThenByDescending(e => ((ITimestampEntity)e).CreatedAt);
        }

        return query.ThenByDescending(e => e.Id);
    }

    public static IQueryable<TEntity> IncludeRange<TEntity>(
        this IQueryable<TEntity> query,
        string[] includePropherties
        ) where TEntity : class, IEntity
    {
        foreach (var propherty in includePropherties)
        {
            query = query.Include(propherty);
        }

        return query;
    }

    public static IQueryable<Category> IncludeHigherCategories(this IQueryable<Category> categoryQueryable, int currentLevel, int toLevel = 0)
    {
        if (currentLevel <= toLevel)
        {
            return categoryQueryable;
        }

        var query = categoryQueryable.Include(e => e.ParentCategory);
        currentLevel -= 1;

        while (currentLevel > toLevel)
        {
            query = query.ThenInclude(e => e!.ParentCategory);
            currentLevel--;
        }

        return query;
    }

    public static IQueryable<Category> IncludeLowerCategories(this IQueryable<Category> categoryQueryable, int currentLevel, int toLevel)
    {
        if (currentLevel >= toLevel)
        {
            return categoryQueryable;
        }

        var query = categoryQueryable.Include(e => e.ChildCategories);
        currentLevel += 1;

        while (currentLevel < toLevel)
        {
            query = query!.ThenInclude(e => e.ChildCategories);
            currentLevel++;
        }

        return query;
    }
}
