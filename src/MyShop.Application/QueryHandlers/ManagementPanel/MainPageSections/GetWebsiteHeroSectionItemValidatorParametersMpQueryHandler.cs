using MyShop.Application.Dtos.ValidatorParameters.ManagementPanel;
using MyShop.Application.Queries.ManagementPanel.MainPageSections;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;

namespace MyShop.Application.QueryHandlers.ManagementPanel.MainPageSections;
internal sealed class GetWebsiteHeroSectionItemValidatorParametersMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetWebsiteHeroSectionItemValidatorParametersMp, ApiResponse<WebsiteHeroSectionItemValidatorParametersMpDto>>
{
    public async Task<ApiResponse<WebsiteHeroSectionItemValidatorParametersMpDto>> HandleAsync(
        GetWebsiteHeroSectionItemValidatorParametersMp query,
        CancellationToken cancellationToken = default
        )
    {
        var maxPositions = await unitOfWork.WebsiteHeroSectionItemRepository.CountAsync(
            predicate: e => e.WebsiteHeroSectionId == query.Id && e.Position != null,
            cancellationToken: cancellationToken
            );

        return new(new(maxPositions + 1));
    }
}
