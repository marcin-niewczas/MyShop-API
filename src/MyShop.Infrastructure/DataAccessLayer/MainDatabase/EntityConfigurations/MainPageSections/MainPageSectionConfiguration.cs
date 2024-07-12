using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.MainPageSections;
using MyShop.Core.ValueObjects.MainPageSections;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.MainPageSections;
internal sealed class MainPageSectionConfiguration : IEntityTypeConfiguration<MainPageSection>
{
    public void Configure(EntityTypeBuilder<MainPageSection> builder)
    {
        builder
           .HasKey(e => e.Id);

        builder
           .Property(e => e.Position)
           .HasMainPageSectionPositionConfiguration();

        builder
            .Property(e => e.MainPageSectionType)
            .HasMainPageSectionTypeConfiguration();

        builder
            .HasDiscriminator(e => e.MainPageSectionType)
            .HasValue<WebsiteHeroSection>(MainPageSectionType.WebsiteHeroSection)
            .HasValue<WebsiteProductsCarouselSection>(MainPageSectionType.WebsiteProductsCarouselSection);
    }
}
