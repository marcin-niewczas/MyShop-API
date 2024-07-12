using MyShop.Core.Dtos.ECommerce;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.MainPageSections;

namespace MyShop.Core.Abstractions.Repositories;
public interface IMainPageSectionRepository : IBaseReadRepository<MainPageSection>
{
    Task<PagedResult<MainPageSectionEcDto>> GetPagedMainPageSectionsEcAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default
        );
}
