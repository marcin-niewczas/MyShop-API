using MyShop.Application.Dtos.ManagementPanel.Photos;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.Photos;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.RepositoryQueryParams.Commons;

namespace MyShop.Application.QueryHandlers.ManagementPanel.Photos;
internal sealed class GetPagedWebsiteHeroSectionPhotosMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedWebsiteHeroSectionPhotosMp, ApiPagedResponse<PhotoMpDto>>
{
    public async Task<ApiPagedResponse<PhotoMpDto>> HandleAsync(
        GetPagedWebsiteHeroSectionPhotosMp query,
        CancellationToken cancellationToken = default
        )
    {
        var pageResult = await unitOfWork.WebsiteHeroSectionPhotoRepository.GetPagedDataAsync(
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            sortByKeySelector: o => o.CreatedAt,
            sortDirection: SortDirection.Descendant,
            cancellationToken: cancellationToken
            );

        return new(
            dtos: pageResult.Data.ToPhotoMpDtos(),
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            totalCount: pageResult.TotalCount
            );
    }
}
