using Microsoft.EntityFrameworkCore.Query;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.RepositoryQueryParams.Commons;
using System.Linq.Expressions;

namespace MyShop.Core.Abstractions.Repositories;
public interface IBaseReadRepository<TEntity> where TEntity : class, IEntity
{
    Task<TEntity?> GetByIdAsync(
        Guid id,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<TEntity?> GetByIdAsync<TPropherty>(
        Guid id,
        Expression<Func<TEntity, TPropherty>> includeExpression,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );

    public Task<TEntity?> GetByIdAsync(
        Guid id,
        IEnumerable<Expression<Func<TEntity, object>>> includeExpressions,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<TEntity?> GetByIdAsync(
        Guid id,
        string[] includePropherties,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<TEntity?> GetByIdAsync(
        Guid id,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<TEntity?> GetFirstByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<TEntity?> GetFirstByPredicateAsync<TPropherty>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TPropherty>> includeExpression,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<TEntity?> GetFirstByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        IEnumerable<Expression<Func<TEntity, object>>> includeExpressions,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<TEntity?> GetFirstByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(
        Expression<Func<TEntity, object>> sortByKeySelector,
        SortDirection sortDirection,
        CancellationToken cancellationToken = default
        );

    Task<IReadOnlyCollection<TEntity>> GetByPredicateAsync<TPropherty>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TPropherty>> includeExpression,
        bool withTracking = false,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyCollection<TEntity>> GetByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        IEnumerable<Expression<Func<TEntity, object>>> includeExpressions,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<IReadOnlyCollection<TResult>> GetByPredicateAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default
        );

    Task<IReadOnlyCollection<TEntity>> GetByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<IReadOnlyCollection<TEntity>> GetByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        string[] includePropherties,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<IReadOnlyCollection<TEntity>> GetByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object?>> sortByKeySelector,
        SortDirection? sortDirection,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<IReadOnlyCollection<TEntity>> GetByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object?>> sortByKeySelector,
        SortDirection? sortDirection,
        Expression<Func<TEntity, object?>> thenSortByKeySelector,
        SortDirection? thenSortDirection,
        bool withTracking = false,
        bool asSplitQuery = false,
        CancellationToken cancellationToken = default
        );

    Task<IReadOnlyCollection<TEntity>> GetByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object?>> sortByKeySelector,
        SortDirection? sortDirection,
        int take,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<IReadOnlyCollection<TEntity>> GetByPredicateAsync<TPropherty>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object?>> sortByKeySelector,
        SortDirection? sortDirection,
        Expression<Func<TEntity, TPropherty>> includeExpression,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<IReadOnlyCollection<TResult>> GetByPredicateWithSelectAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector,
        bool withTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<PagedResult<TEntity>> GetPagedDataAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, object?>> sortByKeySelector,
        SortDirection? sortDirection,
        bool withTracking = false,
        bool asSplitQuery = false,
        CancellationToken cancellationToken = default
        );

    Task<PagedResult<TEntity>> GetPagedDataAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object?>> sortByKeySelector,
        SortDirection? sortDirection,
        bool withTracking = false,
        bool asSplitQuery = false,
        CancellationToken cancellationToken = default
        );

    Task<PagedResult<TEntity>> GetPagedDataAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object?>> sortByKeySelector,
        SortDirection? sortDirection,
        Expression<Func<TEntity, object>> includeExpression,
        bool withTracking = false,
        bool asSplitQuery = false,
        CancellationToken cancellationToken = default
        );

    Task<PagedResult<TEntity>> GetPagedDataAsync(
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
        );

    Task<PagedResult<TEntity>> GetPagedDataAsync(
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
        );

    Task<PagedResult<TEntity>> GetPagedDataAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object?>>? sortByKeySelector,
        SortDirection? sortDirection,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include,
        bool withTracking = false,
        bool asSplitQuery = false,
        CancellationToken cancellationToken = default
        );

    Task<int> SumAsync(
        Expression<Func<TEntity, int>> selector,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
        );

    Task<double> AvarageAsync(
        Expression<Func<TEntity, int>> selector,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
        );

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
        );

    public Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
        );

    Task<bool> AllAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
        );

    public Task<TProperty> MaxAsync<TProperty>(
        Expression<Func<TEntity, TProperty>> selector,
        CancellationToken cancellationToken = default
        );

    Task<TProperty> MaxAsync<TProperty>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TProperty>> selector,
        CancellationToken cancellationToken = default
        );
}
