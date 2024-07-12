using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.MainPageSections;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class WebsiteHeroSectionItemRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<WebsiteHeroSectionItem>(dbContext),
        IWebsiteHeroSectionItemRepository
{
}
