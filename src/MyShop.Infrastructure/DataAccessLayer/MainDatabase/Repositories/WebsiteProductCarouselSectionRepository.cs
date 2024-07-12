using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.MainPageSections;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class WebsiteProductCarouselSectionRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<WebsiteProductsCarouselSection>(dbContext),
        IWebsiteProductCarouselSectionRepository
{
}
