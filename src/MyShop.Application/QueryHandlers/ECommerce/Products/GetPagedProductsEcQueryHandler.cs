using MyShop.Application.Mappings;
using MyShop.Application.Queries.ECommerce.Products;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.Shared;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.RepositoryQueryParams.ECommerce;

namespace MyShop.Application.QueryHandlers.ECommerce.Products;
internal sealed class GetPagedProductsEcQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedProductsEc, ApiPagedResponse<ProductItemDto>>
{
    public async Task<ApiPagedResponse<ProductItemDto>> HandleAsync(GetPagedProductsEc query, CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<Guid>? categoryIds = default;

        if (query.EncodedCategoryName is not null)
        {
            var (queryCategory, theLowestCategories) = await unitOfWork
                .CategoryRepository
                .GetTheLowestCategoriesByEncodedHierarchyNameAsync(query.EncodedCategoryName, cancellationToken: cancellationToken);

            if (queryCategory is null || theLowestCategories is null)
            {
                throw new NotFoundException(nameof(Category), query.EncodedCategoryName);
            }

            categoryIds = theLowestCategories
                .Select(c => c.Id)
                .ToList();
        }

        var productItems = await unitOfWork.ProductVariantRepository.GetPagedDataByCategoryIdsAsync(
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            sortBy: TypeMapper.MapEnum<GetPagedProductsEcSortBy>(query.SortBy),
            categoryIds: categoryIds,
            productOptionParam: query.ProductOptionParam,
            minPrice: query.MinPrice,
            maxPrice: query.MaxPrice,
            searchPhrase: query.SearchPhrase,
            cancellationToken: cancellationToken
            );

        return new(
            dtos: productItems.Data,
            totalCount: productItems.TotalCount,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize
            );
    }
}
