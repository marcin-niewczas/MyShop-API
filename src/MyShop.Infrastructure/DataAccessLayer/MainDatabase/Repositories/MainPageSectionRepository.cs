using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.ECommerce;
using MyShop.Core.Dtos.Shared;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.MainPageSections;
using MyShop.Core.ValueObjects.MainPageSections;
using MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories.Utils;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Repositories;
internal sealed class MainPageSectionRepository(
    MainDbContext dbContext
    ) : BaseGenericRepository<MainPageSection>(dbContext), IMainPageSectionRepository
{
    public Task<PagedResult<MainPageSectionEcDto>> GetPagedMainPageSectionsEcAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default
        ) => _dbSet
            .Where(e => e.Position != null)
            .OrderBy(o => o.Position)
            .Select(e => (MainPageSectionEcDto)(e.MainPageSectionType == MainPageSectionType.WebsiteProductsCarouselSection
                ? new WebsiteProductsCarouselSectionEcDto()
                {
                    MainPageSectionType = e.MainPageSectionType,
                    ProductsCarouselSectionType = ((WebsiteProductsCarouselSection)e).ProductsCarouselSectionType,
                }
                : new WebsiteHeroSectionEcDto()
                {
                    MainPageSectionType = e.MainPageSectionType,
                    DisplayType = ((WebsiteHeroSection)e).DisplayType,
                    Items = ((WebsiteHeroSection)e).WebsiteHeroSectionItems
                        .Where(e => e.Position != null)
                        .OrderBy(o => o.Position)
                        .Select(i => new WebsiteHeroSectionItemEcDto()
                        {
                            Title = i.Title,
                            Subtitle = i.Subtitle,
                            RouterLink = i.RouterLink,
                            Photo = new PhotoDto(i.WebsiteHeroSectionPhoto.Uri, i.WebsiteHeroSectionPhoto.Alt)
                        }).ToArray(),
                })
            )
            .ToPagedResultAsync(
                pageNumber: pageNumber,
                pageSize: pageSize,
                cancellationToken: cancellationToken
            );
}
