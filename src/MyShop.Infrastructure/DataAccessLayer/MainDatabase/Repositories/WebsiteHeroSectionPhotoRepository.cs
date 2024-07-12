using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Photos;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class WebsiteHeroSectionPhotoRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<WebsiteHeroSectionPhoto>(dbContext),
        IWebsiteHeroSectionPhotoRepository
{
}
