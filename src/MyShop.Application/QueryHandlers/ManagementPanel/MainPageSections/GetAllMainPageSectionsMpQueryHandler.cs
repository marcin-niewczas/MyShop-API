using MyShop.Application.Dtos.ManagementPanel.MainPageSections;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.MainPageSections;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.RepositoryQueryParams.Commons;

namespace MyShop.Application.QueryHandlers.ManagementPanel.MainPageSections;
internal sealed class GetAllMainPageSectionsMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetAllMainPageSectionsMp, ApiResponseWithCollection<MainPageSectionMpDto>>
{
    public async Task<ApiResponseWithCollection<MainPageSectionMpDto>> HandleAsync(
        GetAllMainPageSectionsMp query,
        CancellationToken cancellationToken = default
        )
    {
        var result = await unitOfWork.MainPageSectionRepository.GetAllAsync(
            sortByKeySelector: o => o.Position,
            sortDirection: SortDirection.Ascendant,
            cancellationToken: cancellationToken
            );

        return new(result.ToMainPageSectionMpDtos());
    }
}
