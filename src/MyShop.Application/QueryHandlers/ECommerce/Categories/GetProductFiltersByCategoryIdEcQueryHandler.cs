using MyShop.Application.Dtos.ECommerce.Products;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ECommerce.Categories;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.QueryHandlers.ECommerce.Categories;
internal sealed class GetProductFiltersByCategoryIdEcQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetProductFiltersByCategoryIdEc, ApiResponse<ProductFiltersDtoEc>>
{
    public async Task<ApiResponse<ProductFiltersDtoEc>> HandleAsync(GetProductFiltersByCategoryIdEc query, CancellationToken cancellationToken = default)
    {
        var (queryCategory, theLowestCategories) = await unitOfWork.CategoryRepository.GetTheLowestCategoriesByEncodedHierarchyNameAsync(
            query.EncodedCategoryName,
            cancellationToken: cancellationToken
            );

        if (queryCategory is null || theLowestCategories is null)
        {
            throw new NotFoundException(nameof(Category), query.EncodedCategoryName);
        }

        var categoryIds = theLowestCategories
            .Select(c => c.Id)
            .ToList();

        var productFilters = await unitOfWork.ProductRepository.GetProductFiltersEcAsync(
            categoryIds: categoryIds,
            minPrice: query.MinPrice,
            maxPrice: query.MaxPrice,
            productOptionParams: query.ProductOptionParam,
            cancellationToken: cancellationToken
            );

        return new(productFilters.ToProductFiltersEcDto(queryCategory));
    }
}
