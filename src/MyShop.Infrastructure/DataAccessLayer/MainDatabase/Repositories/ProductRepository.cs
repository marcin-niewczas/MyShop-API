using Microsoft.EntityFrameworkCore;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.ProductOptions;
using MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories.Utils;
using System.Linq.Expressions;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class ProductRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<Product>(dbContext), IProductRepository
{
    public Task<Product?> GetProductByIdForCreateProductVariantAsync(
        Guid id,
        CancellationToken cancellationToken = default
        ) => _dbSet
                .Include(i => i.ProductProductVariantOptions.OrderBy(o => o.Position))
                .ThenInclude(i => i.ProductVariantOption)
                .Include(i => i.ProductDetailOptionValues.Where(e => e.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main))
                .ThenInclude(i => i.ProductDetailOption)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);



    public async Task<IReadOnlyCollection<string>> GetProductNamesAsync(
        string searchPhrase,
        int take,
        CancellationToken cancellationToken = default
        )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(searchPhrase);

        var splittedSearchPhrase = searchPhrase.ToLower().Split(" ");

        return await _dbSet
            .Where(e => splittedSearchPhrase.All(sp =>
                            Convert.ToString(e.Name).ToLower().Replace(" ", "").Contains(sp) ||
                            e.Category.HierarchyDetail.HierarchyName.ToLower().Replace(" ", "").Contains(sp) ||
                            e.Category.ParentCategory!.HierarchyDetail.HierarchyName.ToLower().Replace(" ", "").Contains(sp) ||
                            e.Category.ParentCategory!.ParentCategory!.HierarchyDetail.HierarchyName.ToLower().Replace(" ", "").Contains(sp) ||
                            e.ProductDetailOptionValues.Any(v => Convert.ToString(v.Value).ToLower().Replace(" ", "").Contains(sp)) ||
                            e.ProductVariants.Any(pv => pv.ProductVariantOptionValues.Any(v => Convert.ToString(v.Value).ToLower().Replace(" ", "").Contains(sp)))
                            )
                   )
            .Select(e => string.Concat(
                e.ProductDetailOptionValues
                    .First(x => x.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main)
                    .Value,
                " ",
                e.Name
                ))
            .OrderBy(n => n)
            .Take(take)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<ProductMpDto?> GetProductMpAsync(
        Guid id,
        CancellationToken cancellationToken = default
        )
    {
        return _dbSet.Select(e => new ProductMpDto
        {
            Id = e.Id,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt,
            Name = e.Name,
            FullName = string.Concat(
                      e.ProductDetailOptionValues.First(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main).Value,
                      " ",
                      e.Name
                      ),
            Description = e.Description,
            CategorydId = e.CategoryId,
            Category = new CategoryMpDto
            {
                Id = e.Category.Id,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
                Name = e.Category.Name,
                HierarchyName = e.Category.HierarchyDetail.HierarchyName,
                ParentCategoryId = e.Category.ParentCategoryId,
                RootCategoryId = e.Category.HierarchyDetail.RootCategoryId,
                ChildCategories = null,
                Level = e.Category.HierarchyDetail.Level
            },
            DisplayProductType = e.DisplayProductType,
            ProductDetailOptions = e.ProductProductDetailOptionValues
                                    .OrderBy(o => o.Position)
                                    .Select(d => new OptionNameValueId(
                                        d.ProductDetailOptionValue.ProductOptionId,
                                        d.ProductDetailOptionValue.ProductDetailOption.Name,
                                        d.ProductDetailOptionValue.Value
                                        ))
                                    .ToArray(),
            ProductVariantOptions = e.ProductProductVariantOptions
                                    .OrderBy(o => o.Position)
                                    .Select(d => new OptionNameId(
                                        d.ProductVariantOptionId,
                                        d.ProductVariantOption.Name
                                        ))
                                    .ToArray()
        })
        .AsNoTracking()
        .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public Task<PagedResult<PagedProductMpDto>> GetPagedProductsMpAsync(
        int pageNumber,
        int pageSize,
        GetPagedProductsMpSortBy? sortBy,
        SortDirection? sortDirection,
        string? searchPhrase,
        CancellationToken cancellationToken = default
        )
    {
        var splittedSearchPhrase = searchPhrase?.ToLower().Split(" ");

        var baseQuery = _dbSet.Where(e => splittedSearchPhrase.IsNullOrEmpty() ||
                splittedSearchPhrase.All(sp =>
                   Convert.ToString(e.Name).ToLower().Replace(" ", "").Contains(sp) ||
                   e.Category.HierarchyDetail.HierarchyName.ToLower().Replace(" ", "").Contains(sp) ||
                   e.Category.ParentCategory!.HierarchyDetail.HierarchyName.ToLower().Replace(" ", "").Contains(sp) ||
                   e.Category.ParentCategory.ParentCategory!.HierarchyDetail.HierarchyName.ToLower().Replace(" ", "").Contains(sp) ||
                   e.ProductDetailOptionValues.Any(v => Convert.ToString(v.Value).ToLower().Replace(" ", "").Contains(sp)) ||
                   e.ProductVariants.Any(pv => pv.ProductVariantOptionValues.Any(v => Convert.ToString(v.Value).ToLower().Replace(" ", "").Contains(sp)))
                )
              ).Select(e => new PagedProductMpDto
              {
                  Id = e.Id,
                  CreatedAt = e.CreatedAt,
                  UpdatedAt = e.UpdatedAt,
                  Name = e.Name,
                  FullName = string.Concat(
                      e.ProductDetailOptionValues.First(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main).Value,
                      " ",
                      e.Name
                      ),
                  CategorydId = e.CategoryId,
                  Category = new CategoryMpDto
                  {
                      Id = e.Category.Id,
                      CreatedAt = e.CreatedAt,
                      UpdatedAt = e.UpdatedAt,
                      Name = e.Category.Name,
                      HierarchyName = e.Category.HierarchyDetail.HierarchyName,
                      ParentCategoryId = e.Category.ParentCategoryId,
                      RootCategoryId = e.Category.HierarchyDetail.RootCategoryId,
                      ChildCategories = null,
                      Level = e.Category.HierarchyDetail.Level
                  }
              });

        Expression<Func<PagedProductMpDto, object?>> sortByExpression = sortBy switch
        {
            GetPagedProductsMpSortBy.FullName => c => c.FullName,
            GetPagedProductsMpSortBy.Name => c => c.Name,
            GetPagedProductsMpSortBy.Category => c => c.Category.HierarchyName,
            GetPagedProductsMpSortBy.UpdatedAt => c => c.UpdatedAt,
            _ => c => c.CreatedAt,
        };

        baseQuery = sortDirection switch
        {
            SortDirection.Ascendant => baseQuery.OrderBy(sortByExpression),
            _ => baseQuery.OrderByDescending(sortByExpression),
        };

        return baseQuery.ToPagedResultAsync(
            pageNumber,
            pageSize,
            cancellationToken: cancellationToken
            );
    }

    public async Task<ProductFiltersEc> GetProductFiltersEcAsync(
        IEnumerable<Guid> categoryIds,
        decimal? minPrice,
        decimal? maxPrice,
        IReadOnlyDictionary<string, string[]>? productOptionParams,
        CancellationToken cancellationToken = default
        )
    {
        string[]? chosenProductVariantOptions = default;
        string[]? chosenProductVariantOptionKeys = default;
        string[]? chosenProductDetailOptions = default;
        string[]? chosenProductDetailOptionKeys = default;

        if (productOptionParams is not null)
        {
            chosenProductDetailOptionKeys = await _dbContext.ProductDetailOptions
                .Where(e => productOptionParams.Keys.Contains(Convert.ToString(e.Name)))
                .Select(e => e.Name.Value)
                .ToArrayAsync(cancellationToken);

            chosenProductDetailOptions = productOptionParams
                .Where(p => chosenProductDetailOptionKeys.Contains(p.Key))
                .SelectMany(w => w.Value, (x, y) => string.Concat(x.Key, ':', y))
                .ToArray();

            chosenProductVariantOptionKeys = productOptionParams
                .Where(p => !chosenProductDetailOptionKeys.Contains(p.Key))
                .Select(p => p.Key)
                .ToArray();

            chosenProductVariantOptions = productOptionParams
                .Where(p => chosenProductVariantOptionKeys.Contains(p.Key))
                .SelectMany(w => w.Value, (x, y) => string.Concat(x.Key, ':', y))
                .ToArray();

        }

        var productDetailOptions = await _dbContext.ProductDetailOptionValues
            .Where(e => e.Products.Any(x => categoryIds.Contains(x.CategoryId)))
            .Select(e => new
            {
                e.ProductDetailOption,
                e.Value,
                Count = e.Products
                .SelectMany(o => o.ProductVariants)
                .Where(variant => categoryIds.Contains(variant.Product.CategoryId) &&
                       (minPrice == null || variant.Price >= minPrice) &&
                       (maxPrice == null || variant.Price <= maxPrice) &&
                       (chosenProductVariantOptions == null || chosenProductVariantOptions.Count(z => variant.ProductVariantOptionValues.Any(u => z == Convert.ToString(u.ProductVariantOption.Name) + ":" + Convert.ToString(u.Value))) == chosenProductVariantOptionKeys!.Length) &&
                       (chosenProductDetailOptions == null || chosenProductDetailOptionKeys == null || (chosenProductDetailOptionKeys!.Contains(Convert.ToString(e.ProductDetailOption.Name)) ?
                           chosenProductDetailOptions.Where(f => !f.StartsWith(Convert.ToString(e.ProductDetailOption.Name))).Count(t => variant.Product.ProductDetailOptionValues.Any(b => Convert.ToString(b.ProductDetailOption.Name) + ":" + Convert.ToString(b.Value) == t)) == chosenProductDetailOptionKeys.Length - 1 :
                           chosenProductDetailOptions.Count(p => variant.Product.ProductDetailOptionValues.Any(n => Convert.ToString(n.ProductDetailOption.Name) + ":" + Convert.ToString(n.Value) == p)) == chosenProductDetailOptionKeys.Length)))
                .GroupBy(b => b.EncodedName)
                .Count(),
                e.Position

            })
            .GroupBy(w => w.ProductDetailOption)
            .Select(e => new ProductOptionEc()
            {
                Name = e.Key.Name,
                Type = e.Key.ProductOptionType,
                Subtype = e.Key.ProductOptionSubtype,
                Values = e.OrderByDescending(o => o.Count > 0).ThenBy(o => o.Position).Select(x => new ProductOptionValueEc(x.Value, x.Count)).ToList()
            })
            .OrderByDescending(o => o.Subtype)
            .ThenBy(o => o.Name)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var productVariantOptions = await _dbContext.ProductVariantOptionValues
            .Where(e => e.ProductVariants.Any(variant => categoryIds.Contains(variant.Product.CategoryId)))
            .Select(e => new
            {
                e.ProductVariantOption,
                e.Value,
                Count = e.ProductVariants
                .Where(variant => categoryIds.Contains(variant.Product.CategoryId) &&
                       (minPrice == null || variant.Price >= minPrice) &&
                       (maxPrice == null || variant.Price <= maxPrice) &&
                       (chosenProductDetailOptions == null || chosenProductDetailOptions.Count(z => variant.Product.ProductDetailOptionValues.Any(u => z == Convert.ToString(u.ProductDetailOption.Name) + ":" + Convert.ToString(u.Value))) == chosenProductDetailOptionKeys!.Length) &&
                       (chosenProductVariantOptions == null || chosenProductVariantOptionKeys == null || (chosenProductVariantOptionKeys!.Contains(Convert.ToString(e.ProductVariantOption.Name)) ?
                           chosenProductVariantOptions.Where(f => !f.StartsWith(Convert.ToString(e.ProductVariantOption.Name))).Count(t => variant.ProductVariantOptionValues.Any(b => Convert.ToString(b.ProductVariantOption.Name) + ":" + Convert.ToString(b.Value) == t)) == chosenProductVariantOptionKeys.Length - 1 :
                           chosenProductVariantOptions.Count(p => variant.ProductVariantOptionValues.Any(n => Convert.ToString(n.ProductVariantOption.Name) + ":" + Convert.ToString(n.Value) == p)) == chosenProductVariantOptionKeys.Length)))
                .GroupBy(b => b.EncodedName)
                .Count(),
                e.Position
            })
            .GroupBy(w => w.ProductVariantOption)
            .Select(e => new ProductOptionEc()
            {
                Name = e.Key.Name,
                Type = e.Key.ProductOptionType,
                Subtype = e.Key.ProductOptionSubtype,
                Values = e.OrderByDescending(o => o.Count > 0).ThenBy(o => o.Position).Select(x => new ProductOptionValueEc(x.Value, x.Count)).ToList()
            })
            .OrderByDescending(o => o.Subtype)
            .ThenBy(o => o.Name)
            .AsNoTracking()
            .ToListAsync(cancellationToken);


        var minPriceResult = await _dbContext.ProductVariants
            .Where(e => categoryIds.Contains(e.Product.CategoryId))
            .AsNoTracking()
            .MinAsync(p => (decimal?)p.Price, cancellationToken);

        var maxPriceResult = await _dbContext.ProductVariants
            .Where(e => categoryIds.Contains(e.Product.CategoryId))
            .AsNoTracking()
            .MaxAsync(p => (decimal?)p.Price, cancellationToken);


        return new ProductFiltersEc()
        {
            MinPrice = minPriceResult is decimal min ? Math.Floor(min) : minPriceResult,
            MaxPrice = maxPriceResult is decimal max ? Math.Floor(max) : maxPriceResult,
            ProductOptions = [.. productDetailOptions, .. productVariantOptions],
        };
    }
}
