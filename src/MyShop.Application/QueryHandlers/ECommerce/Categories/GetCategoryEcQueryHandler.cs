using MyShop.Application.Dtos.ECommerce.Categories;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ECommerce.Categories;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.QueryHandlers.ECommerce.Categories;
internal sealed class GetCategoryEcQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetCategoryEc, ApiResponse<CategoryEcDto>>
{
    public async Task<ApiResponse<CategoryEcDto>> HandleAsync(
        GetCategoryEc query, 
        CancellationToken cancellationToken = default
        )
    {
        var category = await unitOfWork.CategoryRepository.GetFirstByPredicateAsync(
             predicate: e => e.HierarchyDetail.EncodedHierarchyName == query.EncodedName,
             cancellationToken: cancellationToken
             ) ?? throw new NotFoundException(nameof(Category), query.EncodedName);

        return new(category.ToCategoryEcDto());
    }
}
