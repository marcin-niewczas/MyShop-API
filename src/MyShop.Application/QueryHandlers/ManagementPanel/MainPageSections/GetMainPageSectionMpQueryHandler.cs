using MyShop.Application.Dtos.ManagementPanel.MainPageSections;
using MyShop.Application.Mappings;
using MyShop.Application.Queries.ManagementPanel.MainPageSections;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.MainPageSections;

namespace MyShop.Application.QueryHandlers.ManagementPanel.MainPageSections;
internal sealed class GetMainPageSectionMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetMainPageSectionMp, ApiResponse<MainPageSectionMpDto>>
{
    public async Task<ApiResponse<MainPageSectionMpDto>> HandleAsync(
        GetMainPageSectionMp query,
        CancellationToken cancellationToken = default
        )
    {
        var entity = await unitOfWork.MainPageSectionRepository.GetByIdAsync(
           id: query.Id,
           cancellationToken: cancellationToken
           ) ?? throw new NotFoundException(nameof(MainPageSection), query.Id);

        return new(entity.ToMainPageSectionMpDto());
    }
}
