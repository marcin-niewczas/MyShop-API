using MyShop.Application.Dtos;
using MyShop.Application.Queries.ECommerce.Products;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;

namespace MyShop.Application.QueryHandlers.ECommerce.Products;
internal sealed class GetPagedProductsNamesEcQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedProductsNamesEc, ApiResponse<ValueDto<IReadOnlyCollection<string>>>>
{
    public async Task<ApiResponse<ValueDto<IReadOnlyCollection<string>>>> HandleAsync(
        GetPagedProductsNamesEc query, 
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.ProductRepository.GetProductNamesAsync(
            query.SearchPhrase,
            query.Take,
            cancellationToken
            );

        return new(new(result));
    }
}
