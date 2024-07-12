using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.Categories;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;

namespace MyShop.Application.QueryHandlers.ManagementPanel.Categories;
internal sealed class GetCategoryMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetCategoryMp, ApiResponse<CategoryMpDto>>
{
    public async Task<ApiResponse<CategoryMpDto>> HandleAsync(GetCategoryMp query, CancellationToken cancellationToken = default)
        => new(
                (await unitOfWork.CategoryRepository.GetByIdAsync(
                    query.Id,
                    TypeMapper.MapEnum<GetCategoryMpQueryType>(query.QueryType),
                    cancellationToken
                    ) ?? throw new NotFoundException(nameof(Category), query.Id)).ToCategoryMpDto()
                );
}
