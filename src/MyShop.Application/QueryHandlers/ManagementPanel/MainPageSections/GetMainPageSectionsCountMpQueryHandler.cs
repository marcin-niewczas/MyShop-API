using MyShop.Application.Dtos;
using MyShop.Application.Queries.ManagementPanel.MainPageSections;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;

namespace MyShop.Application.QueryHandlers.ManagementPanel.MainPageSections;
internal sealed class GetMainPageSectionsCountMpQueryHandler(
    IUnitOfWork unitOfWork
    ) : IQueryHandler<GetMainPageSectionsCountMp, ApiResponse<ValueDto<int>>>
{
    public async Task<ApiResponse<ValueDto<int>>> HandleAsync(
        GetMainPageSectionsCountMp query,
        CancellationToken cancellationToken = default
        )
    {
        var count = await unitOfWork.MainPageSectionRepository.CountAsync(cancellationToken);

        return new(new(count));
    }
}
