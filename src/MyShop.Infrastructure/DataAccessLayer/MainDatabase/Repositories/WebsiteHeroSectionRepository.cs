using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.MainPageSections;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class WebsiteHeroSectionRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<WebsiteHeroSection>(dbContext), 
        IWebsiteHeroSectionRepository
{
}
