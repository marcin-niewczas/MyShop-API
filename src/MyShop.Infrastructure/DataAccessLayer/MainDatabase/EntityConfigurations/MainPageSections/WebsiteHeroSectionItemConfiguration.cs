using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.MainPageSections;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.MainPageSections;
internal sealed class WebsiteHeroSectionItemConfiguration : IEntityTypeConfiguration<WebsiteHeroSectionItem>
{
    public void Configure(EntityTypeBuilder<WebsiteHeroSectionItem> builder)
    {
        builder
            .Property(e => e.Position)
            .HasWebsiteHeroSectionItemPositionConfiguration();

        builder
            .Property(e => e.Title)
            .HasWebsiteHeroSectionItemTitleConfiguration();

        builder
            .Property(e => e.Subtitle)
            .HasWebsiteHeroSectionItemSubtitleConfiguration();

        builder
            .Property(e => e.RouterLink)
            .HasWebsiteHeroSectionItemRouterLinkConfiguration();
    }
}
