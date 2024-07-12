using MyShop.Application.Dtos.ManagementPanel.MainPageSections;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.MainPageSections;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.RepositoryQueryParams.Commons;

namespace MyShop.Application.QueryHandlers.ManagementPanel.MainPageSections;
internal sealed class GetPagedWebsiteHeroSectionItemsMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetPagedWebsiteHeroSectionItemsMp, ApiPagedResponse<WebsiteHeroSectionItemMpDto>>
{
    public async Task<ApiPagedResponse<WebsiteHeroSectionItemMpDto>> HandleAsync(
        GetPagedWebsiteHeroSectionItemsMp query,
        CancellationToken cancellationToken = default
        )
    {
        var pagedResult = await (query.Active switch
        {
            true => unitOfWork.WebsiteHeroSectionItemRepository.GetPagedDataAsync(
                pageNumber: query.PageNumber,
                pageSize: query.PageSize,
                predicate: e => e.Position != null && e.WebsiteHeroSectionId == query.Id,
                sortByKeySelector: o => o.Position,
                sortDirection: SortDirection.Ascendant,
                includeExpression: i => i.WebsiteHeroSectionPhoto,
                cancellationToken: cancellationToken
                ),
            _ => unitOfWork.WebsiteHeroSectionItemRepository.GetPagedDataAsync(
               pageNumber: query.PageNumber,
               pageSize: query.PageSize,
               predicate: e => e.Position == null && e.WebsiteHeroSectionId == query.Id,
               sortByKeySelector: o => o.UpdatedAt,
               sortDirection: SortDirection.Descendant,
               thenSortByKeySelector: o => o.CreatedAt,
               thenSortDirection: SortDirection.Descendant,
               includeExpression: i => i.WebsiteHeroSectionPhoto,
               cancellationToken: cancellationToken
               ),
        });

        return new(
            pagedResult.Data.ToWebsiteHeroSectionItemMpDtos(),
            pagedResult.TotalCount,
            query.PageNumber,
            query.PageSize
            );
    }
}
