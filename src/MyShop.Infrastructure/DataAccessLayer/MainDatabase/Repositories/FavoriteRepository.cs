using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.Shared;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;
using MyShop.Core.RepositoryQueryParams.Account;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.ProductOptions;
using MyShop.Core.ValueObjects.Products;
using MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories.Utils;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class FavoriteRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<Favorite>(dbContext), IFavoriteRepository
{
    public async Task<PagedResult<ProductItemDto>> GetPagedFavoritesProductItemsAsync(
        Guid userId,
        int pageNumber,
        int pageSize,
        GetPagedFavoritesAcSortBy? sortBy,
        SortDirection? sortDirection,
        string? searchPhrase = default,
        CancellationToken cancellationToken = default
        )
    {
        var splittedSearchPhrase = searchPhrase?
            .ToLower()
            .Split(" ");

        var baseQuery = _dbSet.AsQueryable();

        baseQuery = (sortBy, sortDirection) switch
        {
            (GetPagedFavoritesAcSortBy.Newest, SortDirection.Ascendant) or (null, null) => baseQuery.OrderBy(e => e.CreatedAt),
            _ => baseQuery.OrderByDescending(e => e.CreatedAt),
        };

        var groupQuery = baseQuery
            .Where(e => e.RegisteredUserId == userId)
            .GroupJoin(
                _dbContext.ProductVariants.Where(e => splittedSearchPhrase.IsNullOrEmpty() ||
                        splittedSearchPhrase.All(sp =>
                           Convert.ToString(e.Product.Name).ToLower().Replace(" ", "").Contains(sp) ||
                           e.Product.Category.HierarchyDetail.HierarchyName.ToLower().Replace(" ", "").Contains(sp) ||
                           e.Product.Category.ParentCategory!.HierarchyDetail.HierarchyName.ToLower().Replace(" ", "").Contains(sp) ||
                           e.Product.Category.ParentCategory.ParentCategory!.HierarchyDetail.HierarchyName.ToLower().Replace(" ", "").Contains(sp) ||
                           e.Product.ProductDetailOptionValues.Any(v => Convert.ToString(v.Value).ToLower().Replace(" ", "").Contains(sp)) ||
                           e.ProductVariantOptionValues.Any(v => Convert.ToString(v.Value).ToLower().Replace(" ", "").Contains(sp))
                           ))
                        .OrderBy(o => o.SortPriority),
                f => f.EncodedProductVariantName,
                pv => pv.EncodedName,
                (favorite, variants) => new { Favorite = favorite, Variants = variants }
                )
            .Where(e => e.Variants.Any())
            .Select(e => new ProductItemDto
            {
                ProductData = e.Variants.AsQueryable().Select(pv => new ProductDataDto
                {
                    ModelName = pv.Product.Name,
                    EncodedName = e.Favorite.EncodedProductVariantName,
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
                    HasMultipleVariants = pv.ProductVariantOptionValues.Count > 1,
                    VariantLabelPositions = pv.ProductVariantOptionValues.Count <= 1
                        ? null
                        : pv.Product.ProductProductVariantOptions
                            .Where(pp => pp.ProductVariantOption.ProductOptionSubtype == ProductOptionSubtype.Additional)
                            .Join(
                                pv.ProductVariantOptionValues,
                                k => k.ProductVariantOptionId,
                                k => k.ProductOptionId,
                                (_, v) => new ValuePosition<string>(v.Value.ToString(), _.Position)).ToList()
                }).First(),

                ProductReviewsCount = e.Variants.First().Product.ProductReviews.Count,
                ProductReviewsRate = e.Variants.First().Product.ProductReviews.Count > 0
                                        ? Math.Round(e.Variants.First().Product.ProductReviews.Average(r => r.Rate), 2)
                                        : 0,
                MinPrice = e.Variants.Min(g => g.Price)!,
                MaxPrice = e.Variants.Max(g => g.Price)!,
                VariantsCount = e.Variants.Count(),
                IsAvailable = e.Variants.Max(x => x.Quantity) != 0,
            });

        groupQuery = (sortBy, sortDirection) switch
        {
            (GetPagedFavoritesAcSortBy.Popular, SortDirection.Ascendant) => groupQuery.OrderBy(e => e.ProductReviewsCount),
            (GetPagedFavoritesAcSortBy.Popular, SortDirection.Descendant) => groupQuery.OrderByDescending(e => e.ProductReviewsCount),
            (GetPagedFavoritesAcSortBy.Rate, SortDirection.Ascendant) => groupQuery.OrderBy(e => e.ProductReviewsRate),
            (GetPagedFavoritesAcSortBy.Rate, SortDirection.Descendant) => groupQuery.OrderByDescending(e => e.ProductReviewsRate),
            (GetPagedFavoritesAcSortBy.Price, SortDirection.Ascendant) => groupQuery.OrderBy(e => e.MinPrice),
            (GetPagedFavoritesAcSortBy.Price, SortDirection.Descendant) => groupQuery.OrderByDescending(e => e.MinPrice),
            _ => groupQuery
        };

        return await groupQuery.ToPagedResultAsync(
            pageNumber,
            pageSize,
            asSplitQuery: true,
            cancellationToken: cancellationToken
        );
    }
}
