using Microsoft.EntityFrameworkCore;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ECommerce;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Dtos.Shared;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;
using MyShop.Core.RepositoryQueryParams.ECommerce;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.ProductOptions;
using MyShop.Core.ValueObjects.Products;
using MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories.Utils;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class ProductVariantRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<ProductVariant>(dbContext), IProductVariantRepository
{
    public Task<ProductVariantMpDto?> GetProductVariantMpAsync(
        Guid id,
        CancellationToken cancellationToken = default
        )
    {
        return _dbSet
            .Select(e => new ProductVariantMpDto
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
                ProductId = e.ProductId,
                Quantity = e.Quantity,
                Price = e.Price,
                EncodedName = e.EncodedName,
                SkuId = e.SkuId,
                ProductName = string.Concat(
                    e.Product.ProductDetailOptionValues.First(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main).Value,
                    " ",
                    e.Product.Name
                    ),
                ProductVariantValues = e.Product
                    .ProductProductVariantOptions
                    .OrderBy(o => o.Position)
                    .Join(e.ProductVariantOptionValues,
                          k => k.ProductVariantOptionId,
                          k => k.ProductOptionId,
                          (option, value) => new OptionNameValueId(
                              option.ProductVariantOptionId,
                              option.ProductVariantOption.Name,
                              value.Value
                              )).ToArray()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    }


    public Task<PagedResult<PagedProductVariantMpDto>> GetPagedProductVariantsMpByProductIdAsync(
        Guid productId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default
        )
    {

        return _dbSet
            .Where(e => e.ProductId == productId)
            .OrderBy(o => o.SortPriority)
            .Select(e => new PagedProductVariantMpDto
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
                Quantity = e.Quantity,
                Price = e.Price,
                EncodedName = e.EncodedName,
                SkuId = e.SkuId,
                ProductVariantValues = e.Product
                    .ProductProductVariantOptions
                    .OrderBy(o => o.Position)
                    .Join(e.ProductVariantOptionValues,
                          k => k.ProductVariantOptionId,
                          k => k.ProductOptionId,
                          (option, value) => new OptionNameValueId(
                              option.ProductVariantOptionId,
                              option.ProductVariantOption.Name,
                              value.Value
                              )).ToArray()
            })
            .AsNoTracking()
            .ToPagedResultAsync(
                pageNumber,
                pageSize,
                asSplitQuery: true,
                cancellationToken: cancellationToken
            );
    }


    public async Task<IReadOnlyCollection<ProductVariantEcDto>> GetProductVariantsByEncodedNameAsync(
        string encodedName,
        CancellationToken cancellationToken = default
        )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(encodedName);

        return await _dbSet
            .Where(e => e.EncodedName == encodedName)
            .Select(pv => new
            {
                pv.Id,
                Values = pv.ProductVariantOptionValues
                            .Where(v => v.ProductVariantOption.ProductOptionSubtype == ProductOptionSubtype.Additional)
                            .AsEnumerable(),
                pv.Quantity,
                pv.Price,
                pv.SortPriority
            })
            .OrderBy(o => o.SortPriority)
            .Select(x => new ProductVariantEcDto(x.Id, string.Join('/', x.Values.Select(l => l.Value)), x.Price, x.Quantity.IsLastItemsInStock()))
            .AsNoTracking()
            .AsSplitQuery()
            .ToArrayAsync(cancellationToken);
    }

    public async Task<BaseProductDetailEcDto?> GetProductDetailAsync(
        string encodedName,
        CancellationToken cancellationToken = default
        )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(encodedName);

        var product = await _dbContext.Products
            .Include(i => i.Category)
            .Include(i => i.ProductProductDetailOptionValues.OrderBy(o => o.Position))
            .ThenInclude(i => i.ProductDetailOptionValue)
            .ThenInclude(i => i.ProductDetailOption)
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(e => e.ProductVariants.Any(v => v.EncodedName == encodedName), cancellationToken);

        if (product is null)
        {
            return null;
        }

        var productReviewsCount = await _dbContext
            .ProductReviews
            .AsNoTracking()
            .CountAsync(e => e.ProductId == product.Id, cancellationToken);

        var sumProductReviewsRate = productReviewsCount > 0
            ? await _dbContext
                .ProductReviews
                .Where(e => e.ProductId == product.Id)
                .AsNoTracking()
                .SumAsync(e => e.Rate, cancellationToken)
            : 0;

        return await (product.DisplayProductType.Value switch
        {
            DisplayProductType.AllVariantOptions => GetProductDetailByAllVariantsAsync(encodedName, product, productReviewsCount, sumProductReviewsRate, cancellationToken),
            DisplayProductType.MainVariantOption => GetProductDetailByMainVariantAsync(encodedName, product, productReviewsCount, sumProductReviewsRate, cancellationToken),
            _ => throw new NotImplementedException($"Not implemented for {nameof(Product.DisplayProductType)} equals '${product.DisplayProductType.Value}'.")
        });
    }

    public async Task<PagedResult<ProductItemDto>> GetPagedDataByCategoryIdsAsync(
       int pageNumber,
       int pageSize,
       GetPagedProductsEcSortBy sortBy,
       IReadOnlyCollection<Guid>? categoryIds,
       IReadOnlyDictionary<string, string[]>? productOptionParam,
       decimal? minPrice,
       decimal? maxPrice,
       string? searchPhrase,
       CancellationToken cancellationToken = default
       )
    {
        string[]? chosenProductVariantOptions = default;
        string[]? chosenProductVariantOptionKeys = default;
        string[]? chosenProductDetailOptions = default;
        string[]? chosenProductDetailOptionKeys = default;

        if (productOptionParam is not null)
        {
            chosenProductDetailOptionKeys = await _dbContext.ProductDetailOptions
                .Where(e => productOptionParam.Keys.Contains(Convert.ToString(e.Name)))
                .Select(e => e.Name.Value)
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

            chosenProductDetailOptions = productOptionParam
                .Where(p => chosenProductDetailOptionKeys.Contains(p.Key))
                .SelectMany(w => w.Value, (x, y) => string.Concat(x.Key, ':', y))
                .ToArray();

            chosenProductVariantOptionKeys = productOptionParam
                .Where(p => !chosenProductDetailOptionKeys.Contains(p.Key))
                .Select(p => p.Key)
                .ToArray();

            chosenProductVariantOptions = productOptionParam
                .Where(p => chosenProductVariantOptionKeys.Contains(p.Key))
                .SelectMany(w => w.Value, (x, y) => string.Concat(x.Key, ':', y))
                .ToArray();

        }

        var splittedSearchPhrase = searchPhrase?
            .ToLower()
            .Split(" ");

        var baseQuery = _dbSet.Where(e => splittedSearchPhrase.IsNullOrEmpty() ||
                splittedSearchPhrase.All(sp =>
                   Convert.ToString(e.Product.Name).ToLower().Replace(" ", "").Contains(sp) ||
                   e.Product.Category.HierarchyDetail.HierarchyName.ToLower().Replace(" ", "").Contains(sp) ||
                   e.Product.Category.ParentCategory!.HierarchyDetail.HierarchyName.ToLower().Replace(" ", "").Contains(sp) ||
                   e.Product.Category.ParentCategory.ParentCategory!.HierarchyDetail.HierarchyName.ToLower().Replace(" ", "").Contains(sp) ||
                   e.Product.ProductDetailOptionValues.Any(v => Convert.ToString(v.Value).ToLower().Replace(" ", "").Contains(sp)) ||
                   e.ProductVariantOptionValues.Any(v => Convert.ToString(v.Value).ToLower().Replace(" ", "").Contains(sp))
                )
              );
        //.Where(e => minPrice == null || e.Price >= minPrice)
        //.Where(e => maxPrice == null || e.Price <= maxPrice);     

        if (sortBy is GetPagedProductsEcSortBy.Bestsellers)
        {
            baseQuery = baseQuery.OrderByDescending(o => o.OrderProducts.Sum(op => op.Quantity));
        }

        var groupQuery = baseQuery
            .GroupBy(z => z.EncodedName);

        if (sortBy is GetPagedProductsEcSortBy.Newest)
        {
            groupQuery = groupQuery.OrderByDescending(o => o.Max(m => m.CreatedAt));
        }

        var finalQuery = groupQuery.Where(e => categoryIds == null || e.Any(v => categoryIds.Contains(v.Product.CategoryId)))
            .Where(e => chosenProductVariantOptions == null ||
                        chosenProductVariantOptionKeys == null ||
                        e.Any(o => o.ProductVariantOptionValues.Count(v => chosenProductVariantOptions.Contains(Convert.ToString(v.ProductVariantOption.Name) + ":" + Convert.ToString(v.Value))) == chosenProductVariantOptionKeys.Length)
                        )
            .Where(e => chosenProductDetailOptions == null ||
                        chosenProductDetailOptionKeys == null ||
                        e.Any(o => o.Product.ProductDetailOptionValues.Count(v => chosenProductDetailOptions.Contains(Convert.ToString(v.ProductDetailOption.Name) + ":" + Convert.ToString(v.Value))) == chosenProductDetailOptionKeys.Length)
                        )
            .Where(e => (minPrice == null && maxPrice == null) ||
                        (minPrice != null && maxPrice != null && e.Any(v => v.Price >= minPrice && v.Price <= maxPrice)) ||
                        (minPrice != null && maxPrice == null && e.Any(v => v.Price >= minPrice)) ||
                        (maxPrice != null && minPrice == null && e.Any(v => v.Price <= maxPrice))
                        )
            .Select(g => new ProductItemDto
            {
                ProductData = g.AsQueryable().OrderBy(o => o.SortPriority).Select(pv => new ProductDataDto
                {
                    ModelName = pv.Product.Name,
                    EncodedName = g.Key,
                    MainPhoto = pv.PhotoItems
                        .Where(item => item.Position == 0)
                        .Select(item => new PhotoDto(item.ProductVariantPhoto.Uri, item.ProductVariantPhoto.Alt))
                        .FirstOrDefault(),
                    ProductVariantId = pv.Product.DisplayProductType == DisplayProductType.AllVariantOptions ? pv.Id : null,
                    DisplayProductPer = pv.Product.DisplayProductType,
                    CategoryHierarchyName = pv.Product.Category.HierarchyDetail.HierarchyName,
                    MainDetailOptionValue = pv.Product
                        .ProductDetailOptionValues
                        .First(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main).Value,
                    MainVariantOptionValue = pv.ProductVariantOptionValues
                        .First(v => v.ProductVariantOption.ProductOptionSubtype == ProductOptionSubtype.Main).Value,
                    HasMultipleVariants = pv.ProductVariantOptionValues.Count > 2,
                    VariantLabelPositions = pv.ProductVariantOptionValues.Count <= 1
                        ? null
                        : pv.Product.ProductProductVariantOptions
                            .Where(pp => pp.ProductVariantOption.ProductOptionSubtype == ProductOptionSubtype.Additional)
                            .Join(
                                pv.ProductVariantOptionValues,
                                k => k.ProductVariantOptionId,
                                k => k.ProductOptionId,
                                (ppvo, v) => new ValuePosition<string>(v.Value.ToString(), ppvo.Position)).ToList()

                }).First(),

                ProductReviewsCount = g.First().Product.ProductReviews.Count,
                ProductReviewsRate = g.First().Product.ProductReviews.Count > 0
                                        ? Math.Round(g.First().Product.ProductReviews.Average(r => r.Rate), 2)
                                        : 0,
                MinPrice = g.Min(g => g.Price)!,
                MaxPrice = g.Max(g => g.Price)!,
                VariantsCount = g.Count(),
                IsAvailable = g.Max(x => x.Quantity) != 0,

            });

        finalQuery = sortBy switch
        {
            GetPagedProductsEcSortBy.TopRated => finalQuery.OrderByDescending(g => g.ProductReviewsRate),
            GetPagedProductsEcSortBy.MostReviewed => finalQuery.OrderByDescending(g => g.ProductReviewsCount),
            GetPagedProductsEcSortBy.PriceAsc => finalQuery.OrderBy(g => g.MinPrice),
            GetPagedProductsEcSortBy.PriceDesc => finalQuery.OrderByDescending(g => g.MinPrice),
            _ => finalQuery
        };

        return await finalQuery.ToPagedResultAsync(
            pageNumber,
            pageSize,
            cancellationToken: cancellationToken
            );
    }

    private async Task<BaseProductDetailEcDto?> GetProductDetailByAllVariantsAsync(
        string encodedName,
        Product product,
        int productReviewsCount,
        int sumProductReviewsRate,
        CancellationToken cancellationToken = default
        )
    {
        var availableVariants = await _dbContext
            .ProductVariantOptionValues
            .Where(e => e.ProductVariants.Any(pv => pv.ProductId == product.Id))
            .Join(_dbContext.ProductProductVariantOptions.Where(e => e.ProductId == product.Id),
                  k => k.ProductOptionId,
                  k => k.ProductVariantOptionId,
                  (v, pvov) => new { pvov.Position, ProductVariantOptionValue = v })
            .GroupBy(e => new { e.Position, e.ProductVariantOptionValue.ProductVariantOption.Name })
            .OrderBy(o => o.Key.Position)
            .Select(x => new AvailableOptionByAllEc(
                    x.Key.Name,
                    x.AsQueryable()
                    .OrderBy(o => o.ProductVariantOptionValue.Position)
                    .Select(v => new VariantByEc<string>
                    {
                        Value = v.ProductVariantOptionValue.Value,
                        EncodedName = v.ProductVariantOptionValue
                        .ProductVariants
                        .OrderBy(o => o.SortPriority)
                        .First(f => f.ProductId == product.Id)
                        .EncodedName,
                    })
                    .ToList()
                    ))
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (availableVariants.Count <= 0)
        {
            return null;
        }

        var allVariants = await _dbSet
            .Where(e => e.ProductId == product.Id)
            .Select(x => new VariantByEc<IReadOnlyCollection<OptionNameValue>>
            {
                EncodedName = x.EncodedName,
                Value = x
                .ProductVariantOptionValues
                .Select(v => new OptionNameValue(v.ProductVariantOption.Name, v.Value))
                .ToList()

            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (allVariants.Count <= 0)
        {
            return null;
        }

        var currentVariant = await _dbSet
            .Where(e => e.EncodedName == encodedName)
            .Select(e => new CurrentVariantByAllEc(
                e.Id,
                e.Price,
                e.Quantity.IsLastItemsInStock(),
                e.Product
                 .ProductProductVariantOptions
                 .OrderBy(o => o.Position)
                 .Join(e.ProductVariantOptionValues,
                       k => k.ProductVariantOptionId,
                       k => k.ProductOptionId,
                       (_, v) => new ProductOptionNameValue(
                           v.ProductVariantOption.Name,
                           v.Value,
                           v.ProductVariantOption.ProductOptionSubtype
                           )
                 ).ToArray(),
                e.PhotoItems
                 .OrderBy(o => o.Position)
                 .Select(p => new PhotoDto(p.ProductVariantPhoto.Uri, p.ProductVariantPhoto.Alt))
                 .ToArray()
                )
           )
            .AsSplitQuery()
            .AsNoTracking()
            .FirstAsync(cancellationToken);

        return new AllVariantProductDetailEcDto(
            encodedName,
            product,
            productReviewsCount,
            sumProductReviewsRate,
            availableVariants,
            currentVariant,
            allVariants
            );
    }

    private async Task<BaseProductDetailEcDto?> GetProductDetailByMainVariantAsync(
        string encodedName,
        Product product,
        int productReviewsCount,
        int sumProductReviewsRate,
        CancellationToken cancellationToken = default
        )
    {
        var availableVariants = await _dbContext
            .ProductVariantOptionValues
            .Where(e => e.ProductVariants.Any(pv => pv.ProductId == product.Id && pv.EncodedName == encodedName)
                    && e.ProductVariantOption.ProductOptionSubtype == ProductOptionSubtype.Additional)
            .Join(_dbContext.ProductProductVariantOptions.Where(e => e.ProductId == product.Id),
                  k => k.ProductOptionId,
                  k => k.ProductVariantOptionId,
                  (v, pvov) => new { pvov.Position, ProductVariantOptionValue = v })
            .GroupBy(e => new { e.Position, e.ProductVariantOptionValue.ProductVariantOption.Name })
            .OrderBy(o => o.Key.Position)
            .Select(x => new AvailableOptionByMainDto(
                    x.Key.Name,
                    x.AsQueryable()
                    .OrderBy(o => o.ProductVariantOptionValue.Position)
                    .Select(v => v.ProductVariantOptionValue.Value)
                    .Cast<string>()
                    .ToArray()
                    ))
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync(cancellationToken);


        var currentVariants = await _dbSet
            .Include(i => i.ProductVariantOptionValues)
            .ThenInclude(i => i.ProductVariantOption)
            .Include(i => i.PhotoItems.OrderBy(p => p.Position))
            .ThenInclude(i => i.ProductVariantPhoto)
            .Where(v => v.ProductId == product.Id && v.EncodedName == encodedName)
            .OrderBy(o => o.SortPriority)
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        if (currentVariants.IsNullOrEmpty())
        {
            return null;
        }

        var currentVariantsDtos = currentVariants.Select(x => new CurrentVariantByMainEcDto(
            x.Id,
            x.Price,
            x.Quantity.IsLastItemsInStock(),
            x.ProductVariantOptionValues.Where(v => v.ProductVariantOption.ProductOptionSubtype == ProductOptionSubtype.Main)
                    .Select(v => new OptionNameValue(v.ProductVariantOption.Name, v.Value))
                    .First(),
                x.ProductVariantOptionValues.Where(v => v.ProductVariantOption.ProductOptionSubtype == ProductOptionSubtype.Additional)
                    .Select(v => new OptionNameValue(v.ProductVariantOption.Name, v.Value))
                    .ToArray(),
            x.PhotoItems.Select(p => new PhotoDto(p.ProductVariantPhoto.Uri, p.ProductVariantPhoto.Alt)).ToArray()
            )).ToArray();

        var otherVariants = await _dbContext
        .Products
        .Where(e => e.ProductVariants.Any(v => v.ProductId == product.Id && v.EncodedName == encodedName))
        .SelectMany(x => x.ProductVariants)
        .GroupBy(g => g.EncodedName)
        .Select(x => new VariantByEc<OptionNameValue>
        {
            EncodedName = x.Key,
            Value = x
                .First()
                .ProductVariantOptionValues
                .Where(v => v.ProductVariantOption.ProductOptionSubtype == ProductOptionSubtype.Main)
                .Select(v => new OptionNameValue(v.ProductVariantOption.Name, v.Value))
                .First(),
        })
        .OrderBy(o => o.EncodedName)
        .AsSplitQuery()
        .AsNoTracking()
        .ToListAsync(cancellationToken);

        var minPrice = await _dbSet
            .Where(e => e.ProductId == product.Id && e.EncodedName == encodedName)
            .MinAsync(x => x.Price, cancellationToken);

        var maxPrice = await _dbSet
            .Where(e => e.ProductId == product.Id && e.EncodedName == encodedName)
            .MaxAsync(x => x.Price, cancellationToken);

        return new MainVariantProductDetailEcDto(
            encodedName,
            product,
            productReviewsCount,
            sumProductReviewsRate,
            availableVariants,
            minPrice,
            maxPrice,
            currentVariantsDtos,
            otherVariants,
            encodedName
            );
    }
}
