using MyShop.Application.Dtos.ManagementPanel.ProductOptionValues;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.ProductOptions;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;

namespace MyShop.Application.QueryHandlers.ManagementPanel.ProductOptions;
internal sealed class GetPagedProductOptionValuesByProductOptionIdMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedProductOptionValuesByProductOptionIdMp, ApiPagedResponse<ProductOptionValueMpDto>>
{
    public async Task<ApiPagedResponse<ProductOptionValueMpDto>> HandleAsync(
        GetPagedProductOptionValuesByProductOptionIdMp query, 
        CancellationToken cancellationToken = default
        )
    {
        var pagedResult = await unitOfWork.BaseProductOptionValueRepository.GetPagedDataByProductOptionIdAsync(
            query.Id,
            query.PageNumber,
            query.PageSize,
            query.SearchPhrase,
            cancellationToken
            );

        return new(
            pagedResult.Data.ToProductOptionValueMpDtos(),
            pagedResult.TotalCount,
            query.PageNumber,
            query.PageSize
           );
    }
}
