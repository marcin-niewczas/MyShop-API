using MyShop.Application.Validations.Validators;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;
using MyShop.Core.ValueObjects.ProductOptions;
using MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories.Utils;
using System.Linq.Expressions;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class BaseProductOptionRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<BaseProductOption>(dbContext),
        IBaseProductOptionRepository
{
    public Task<PagedResult<BaseProductOption>> GetPagedDataAsync(
        int pageNumber,
        int pageSize,
        ProductOptionTypeMpQueryType productOptionTypeQueryType,
        ProductOptionSubtypeMpQueryType productOptionSubtypeQueryType,
        GetPagedProductOptionsMpSortBy? sortBy,
        SortDirection? sortDirection,
        string? searchPhrase,
        CancellationToken cancellationToken = default
        )
    {
        var baseQuery = productOptionTypeQueryType switch
        {
            ProductOptionTypeMpQueryType.All => _dbSet,
            ProductOptionTypeMpQueryType.Variant => _dbSet.Where(b => b.ProductOptionType == ProductOptionType.Variant),
            ProductOptionTypeMpQueryType.Detail => _dbSet.Where(b => b.ProductOptionType == ProductOptionType.Detail),
            _ => throw new ArgumentException(CustomValidators.Enums.GetEnumErrorMessage<ProductOptionTypeMpQueryType>(nameof(productOptionSubtypeQueryType)))
        };

        baseQuery = productOptionSubtypeQueryType switch
        {
            ProductOptionSubtypeMpQueryType.All => baseQuery.Where(b => searchPhrase == null || Convert.ToString(b.Name).ToLower().Contains(searchPhrase.ToLower())),
            ProductOptionSubtypeMpQueryType.Main => baseQuery.Where(b => b.ProductOptionSubtype == ProductOptionSubtype.Main && (searchPhrase == null || b.Name.ToLower().Contains(searchPhrase.ToLower()))),
            ProductOptionSubtypeMpQueryType.Additional => baseQuery.Where(b => b.ProductOptionSubtype == ProductOptionSubtype.Additional && (searchPhrase == null || b.Name.ToLower().Contains(searchPhrase.ToLower()))),
            _ => throw new ArgumentException(CustomValidators.Enums.GetEnumErrorMessage<ProductOptionSubtypeMpQueryType>(nameof(productOptionSubtypeQueryType)))
        };

        Expression<Func<BaseProductOption, object?>> sortByExpression = sortBy switch
        {
            GetPagedProductOptionsMpSortBy.Name => x => x.Name,
            GetPagedProductOptionsMpSortBy.ProductOptionSubtype => x => x.ProductOptionSubtype,
            GetPagedProductOptionsMpSortBy.ProductOptionSortType => x => x.ProductOptionSortType,
            GetPagedProductOptionsMpSortBy.UpdatedAt => x => x.UpdatedAt,
            _ => x => x.CreatedAt
        };

        baseQuery = sortDirection switch
        {
            SortDirection.Ascendant => baseQuery.OrderBy(sortByExpression),
            _ => baseQuery.OrderByDescending(sortByExpression)
        };

        return baseQuery.ToPagedResultAsync(
            pageNumber,
            pageSize,
            cancellationToken: cancellationToken
            );
    }
}
