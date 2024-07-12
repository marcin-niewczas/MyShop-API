using MyShop.Application.Dtos.ManagementPanel.MainPageSections;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.MainPageSections;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.MainPageSections;

namespace MyShop.Application.QueryHandlers.ManagementPanel.MainPageSections;
internal sealed class GetWebsiteHeroSectionItemMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetWebsiteHeroSectionItemMp, ApiResponse<WebsiteHeroSectionItemMpDto>>
{
    public async Task<ApiResponse<WebsiteHeroSectionItemMpDto>> HandleAsync(
        GetWebsiteHeroSectionItemMp query,
        CancellationToken cancellationToken = default
        )
    {
        var entity = await unitOfWork.WebsiteHeroSectionItemRepository.GetByIdAsync(
            id: query.Id,
            includeExpression: i => i.WebsiteHeroSectionPhoto,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(WebsiteHeroSectionItem), query.Id);

        return new(entity.ToWebsiteHeroSectionItemMpDto());
    }
}
