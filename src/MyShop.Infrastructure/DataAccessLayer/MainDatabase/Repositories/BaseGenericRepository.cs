using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories.Utils;
using System.Linq.Expressions;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal abstract class BaseGenericRepository<TEntity>
    : BaseRepository,
      IBaseReadRepository<TEntity>,
      IBaseWriteRepository<TEntity>
      where TEntity : class, IEntity
{
    protected readonly DbSet<TEntity> _dbSet;

    public BaseGenericRepository(MainDbContext dbContext) : base(dbContext)
    {
        _dbSet = _dbContext.Set<TEntity>();
    }

    #region ReadRepository

    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default
        ) => await _dbSet
                    .AsNoTracking()
                    .ToArrayAsync(cancellationToken);

    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(
        Expression<Func<TEntity, object>> sortByKeySelector,
        SortDirection sortDirection,
        CancellationToken cancellationToken = default
        ) => await (sortDirection switch
        {
            SortDirection.Ascendant => _dbSet.OrderBy(sortByKeySelector),
            SortDirection.Descendant => _dbSet.OrderByDescending(sortByKeySelector),
            _ => throw new NotImplementedException()
        }).AsNoTracking().ToArrayAsync(cancellationToken);


    public Task<TEntity?> GetByIdAsync(
        Guid id,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        ) => (withTracking switch
        {
            true => _dbSet,
            _ => _dbSet.AsNoTracking()
        }).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public Task<TEntity?> GetByIdAsync(
        Guid id,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        )
    {
        var baseQuery = include(_dbSet).AsQueryable();

        if (!withTracking)
        {
            baseQuery = baseQuery.AsNoTracking();
        }

        return baseQuery.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public Task<TEntity?> GetByIdAsync<TPropherty>(
        Guid id,
        Expression<Func<TEntity, TPropherty>> includeExpression,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        ) => withTracking switch
        {
            true => _dbSet.Include(includeExpression).FirstOrDefaultAsync(e => e.Id == id, cancellationToken),
            _ => _dbSet.Include(includeExpression).AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
        };

    public Task<TEntity?> GetByIdAsync(
        Guid id,
        IEnumerable<Expression<Func<TEntity, object>>> includeExpressions,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        )
    {
        var baseQuery = _dbSet.AsQueryable();

        foreach (var include in includeExpressions)
        {
            baseQuery = baseQuery.Include(include);
        }

        if (!withTracking)
        {
            baseQuery = baseQuery.AsNoTracking();
        }

        return baseQuery.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public Task<TEntity?> GetByIdAsync(
        Guid id,
        string[] includePropherties,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        ) => withTracking switch
        {
            true => _dbSet.IncludeRange(includePropherties).FirstOrDefaultAsync(e => e.Id == id, cancellationToken),
            _ => _dbSet.IncludeRange(includePropherties).AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
        };

    public async Task<IReadOnlyCollection<TEntity>> GetByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        string[] includePropherties,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        ) => await (withTracking switch
        {
            true => _dbSet.Where(predicate).IncludeRange(includePropherties).ToListAsync(cancellationToken),
            _ => _dbSet.Where(predicate).IncludeRange(includePropherties).AsNoTracking().ToListAsync(cancellationToken)
        });

    public async Task<IReadOnlyCollection<TEntity>> GetByPredicateAsync<TPropherty>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TPropherty>> includeExpression,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        ) => await (withTracking switch
        {
            true => _dbSet.Include(includeExpression).Where(predicate).ToListAsync(cancellationToken),
            _ => _dbSet.Include(includeExpression).Where(predicate).AsNoTracking().ToListAsync(cancellationToken)
        });

    public async Task<IReadOnlyCollection<TEntity>> GetByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        IEnumerable<Expression<Func<TEntity, object>>> includeExpressions,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        )
    {
        var baseQuery = _dbSet.AsQueryable();

        foreach (var include in includeExpressions)
        {
            baseQuery = baseQuery.Include(include);
        }

        if (!withTracking)
        {
            baseQuery = baseQuery.AsNoTracking();
        }

        return await baseQuery
            .Where(predicate)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TResult>> GetByPredicateAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default
        ) => await _dbSet
                .Where(predicate)
                .AsNoTracking()
                .Select(selector)
                .ToArrayAsync(cancellationToken);


    public async Task<IReadOnlyCollection<TEntity>> GetByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        )
    {
        var baseQuery = _dbSet.AsQueryable();

        if (include is not null)
        {
            baseQuery = include(baseQuery);
        }

        if (!withTracking)
        {
            baseQuery = baseQuery.AsNoTracking();
        }

        return await baseQuery
            .Where(predicate)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TEntity>> GetByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object?>> sortByKeySelector,
        SortDirection? sortDirection,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        ) => await (withTracking switch
        {
            true => _dbSet.Where(predicate).ApplySort(sortByKeySelector, sortDirection).ToListAsync(cancellationToken),
            _ => _dbSet.Where(predicate).ApplySort(sortByKeySelector, sortDirection).AsNoTracking().ToListAsync(cancellationToken)
        });

    public async Task<IReadOnlyCollection<TEntity>> GetByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object?>> sortByKeySelector,
        SortDirection? sortDirection,
        Expression<Func<TEntity, object?>> thenSortByKeySelector,
        SortDirection? thenSortDirection,
        bool withTracking = false,
        bool asSplitQuery = false,
        CancellationToken cancellationToken = default
        )
    {
        var query = _dbSet
            .Where(predicate)
            .ApplySort(sortByKeySelector, sortDirection)
            .ApplyThenSort(thenSortByKeySelector, thenSortDirection);

        if (!withTracking)
        {
            query = query.AsNoTracking();
        }

        if (asSplitQuery)
        {
            query = query.AsSplitQuery();
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TEntity>> GetByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object?>> sortByKeySelector,
        SortDirection? sortDirection,
        int take,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        ) => await (withTracking switch
        {
            true => _dbSet.Where(predicate).ApplySort(sortByKeySelector, sortDirection).Take(take).ToListAsync(cancellationToken),
            _ => _dbSet.Where(predicate).ApplySort(sortByKeySelector, sortDirection).Take(take).AsNoTracking().ToListAsync(cancellationToken)
        });

    public async Task<IReadOnlyCollection<TEntity>> GetByPredicateAsync<TPropherty>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object?>> sortByKeySelector,
        SortDirection? sortDirection,
        Expression<Func<TEntity, TPropherty>> includeExpression,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        ) => await (withTracking switch
        {
            true => _dbSet.Include(includeExpression).Where(predicate).ApplySort(sortByKeySelector, sortDirection).ToListAsync(cancellationToken),
            _ => _dbSet.Include(includeExpression).Where(predicate).ApplySort(sortByKeySelector, sortDirection).AsNoTracking().ToListAsync(cancellationToken)
        });

    public async Task<IReadOnlyCollection<TResult>> GetByPredicateWithSelectAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        ) => await (withTracking switch
        {
            true => _dbSet.Where(predicate).Select(selector).ToListAsync(cancellationToken),
            _ => _dbSet.Where(predicate).AsNoTracking().Select(selector).ToListAsync(cancellationToken)
        });

    public Task<TEntity?> GetFirstByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        ) => withTracking switch
        {
            true => _dbSet.FirstOrDefaultAsync(predicate, cancellationToken),
            _ => _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken)
        };

    public Task<TEntity?> GetFirstByPredicateAsync<TPropherty>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TPropherty>> includeExpression,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        ) => withTracking switch
        {
            true => _dbSet.Include(includeExpression).FirstOrDefaultAsync(predicate, cancellationToken),
            _ => _dbSet.Include(includeExpression).AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken)
        };

    public Task<TEntity?> GetFirstByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        IEnumerable<Expression<Func<TEntity, object>>> includeExpressions,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        )
    {
        var baseQuery = _dbSet.AsQueryable();

        foreach (var include in includeExpressions)
        {
            baseQuery = baseQuery.Include(include);
        }

        if (!withTracking)
        {
            baseQuery = baseQuery.AsNoTracking();
        }

        return baseQuery
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<TEntity?> GetFirstByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        )
    {
        var baseQuery = include(_dbSet).AsQueryable();

        if (!withTracking)
        {
            baseQuery = baseQuery.AsNoTracking();
        }

        return baseQuery
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<PagedResult<TEntity>> GetPagedDataAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, object?>> sortByKeySelector,
        SortDirection? sortDirection,
        bool withTracking = false,
        bool asSplitQuery = false,
        CancellationToken cancellationToken = default
        )
    {
        var query = _dbSet.ApplySort(sortByKeySelector, sortDirection).AsQueryable();

        if (!withTracking)
        {
            query = query.AsNoTracking();
        }

        return query.ToPagedResultAsync(pageNumber, pageSize, asSplitQuery, cancellationToken);
    }

    public Task<PagedResult<TEntity>> GetPagedDataAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object?>> sortByKeySelector,
        SortDirection? sortDirection,
        bool withTracking = false,
        bool asSplitQuery = false,
        CancellationToken cancellationToken = default
        )
    {
        var query = _dbSet
            .Where(predicate)
            .ApplySort(sortByKeySelector, sortDirection).AsQueryable();

        if (!withTracking)
        {
            query = query.AsNoTracking();
        }

        return query.ToPagedResultAsync(pageNumber, pageSize, asSplitQuery, cancellationToken);
    }

    public Task<PagedResult<TEntity>> GetPagedDataAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object?>> sortByKeySelector,
        SortDirection? sortDirection,
        Expression<Func<TEntity, object>> includeExpression,
        bool withTracking = false,
        bool asSplitQuery = false,
        CancellationToken cancellationToken = default
        )
    {
        var query = _dbSet
            .Include(includeExpression)
            .Where(predicate)
            .ApplySort(sortByKeySelector, sortDirection).AsQueryable();

        if (!withTracking)
        {
            query = query.AsNoTracking();
        }

        return query.ToPagedResultAsync(pageNumber, pageSize, asSplitQuery, cancellationToken);
    }

    public Task<PagedResult<TEntity>> GetPagedDataAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object?>> sortByKeySelector,
        SortDirection? sortDirection,
        Expression<Func<TEntity, object?>> thenSortByKeySelector,
        SortDirection? thenSortDirection,
        Expression<Func<TEntity, object>> includeExpression,
        bool withTracking = false,
        bool asSplitQuery = false,
        CancellationToken cancellationToken = default
        )
    {
        var query = _dbSet
            .Include(includeExpression)
            .Where(predicate)
            .ApplySort(sortByKeySelector, sortDirection)
            .ApplyThenSort(thenSortByKeySelector, thenSortDirection);

        if (!withTracking)
        {
            query = query.AsNoTracking();
        }

        return query.ToPagedResultAsync(pageNumber, pageSize, asSplitQuery, cancellationToken);
    }



    public Task<PagedResult<TEntity>> GetPagedDataAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object?>> sortByKeySelector,
        SortDirection? sortDirection,
        Expression<Func<TEntity, object?>> thenSortByKeySelector,
        SortDirection? thenSortDirection,
        bool withTracking = false,
        bool asSplitQuery = false,
        CancellationToken cancellationToken = default
        )
    {
        var query = _dbSet
            .Where(predicate)
            .ApplySort(sortByKeySelector, sortDirection)
            .ApplyThenSort(thenSortByKeySelector, thenSortDirection);

        if (!withTracking)
        {
            query = query.AsNoTracking();
        }

        return query.ToPagedResultAsync(pageNumber, pageSize, asSplitQuery, cancellationToken);
    }

    public Task<PagedResult<TEntity>> GetPagedDataAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object?>>? sortByKeySelector,
        SortDirection? sortDirection,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include,
        bool withTracking = false,
        bool asSplitQuery = false,
        CancellationToken cancellationToken = default
        )
    {
        var baseQuery = _dbSet.AsQueryable();


        baseQuery = include(baseQuery);

        baseQuery = baseQuery
            .Where(predicate);

        if (!withTracking)
        {
            baseQuery = baseQuery.AsNoTracking();
        }

        return baseQuery
                    .ApplySort(sortByKeySelector, sortDirection)
                    .ToPagedResultAsync(
                        pageNumber,
                        pageSize,
                        asSplitQuery,
                        cancellationToken
                    );
    }

    public Task<int> SumAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken = default)
        => _dbSet
        .AsNoTracking()
        .SumAsync(selector, cancellationToken);

    public Task<int> SumAsync(
        Expression<Func<TEntity, int>> selector,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
        ) => _dbSet
        .Where(predicate)
        .AsNoTracking()
        .SumAsync(selector, cancellationToken);

    public Task<double> AvarageAsync(
        Expression<Func<TEntity, int>> selector,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
        ) => _dbSet
        .Where(predicate)
        .AsNoTracking()
        .AverageAsync(selector, cancellationToken);

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
        => _dbSet.CountAsync(cancellationToken);

    public Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate,
        CancellationToken cancellationToken = default
        ) => predicate is null ? CountAsync(cancellationToken) : _dbSet.CountAsync(predicate, cancellationToken);

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => _dbSet
            .AsNoTracking()
            .AnyAsync(predicate, cancellationToken);

    public Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => _dbSet
            .AsNoTracking()
            .AllAsync(predicate, cancellationToken);

    public Task<TProperty> MaxAsync<TProperty>(
        Expression<Func<TEntity, TProperty>> selector,
        CancellationToken cancellationToken = default
        ) => _dbSet
            .AsNoTracking()
            .MaxAsync(selector, cancellationToken);

    public Task<TProperty> MaxAsync<TProperty>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TProperty>> selector,
        CancellationToken cancellationToken = default
        ) => _dbSet
            .Where(predicate)
            .AsNoTracking()
            .MaxAsync(selector, cancellationToken);

    #endregion

    #region WriteRepository

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => (await _dbSet.AddAsync(entity, cancellationToken)).Entity;

    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        => _dbSet.AddRangeAsync(entities, cancellationToken);

    public Task<int> ExecuteUpdateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
        CancellationToken cancellationToken = default
        ) => _dbSet
            .Where(predicate)
            .ExecuteUpdateAsync(setPropertyCalls, cancellationToken);

    public Task<TEntity> UpdateAsync(TEntity entity)
    {
        var updatedEntity = _dbSet.Update(entity).Entity;
        return Task.FromResult(updatedEntity);
    }

    public Task UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public Task RemoveRangeAsync(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }

    #endregion
}
