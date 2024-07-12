using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.MainPageSections;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.MainPageSections;
internal sealed class WebsiteHeroSectionConfiguration : IEntityTypeConfiguration<WebsiteHeroSection>
{
    public void Configure(EntityTypeBuilder<WebsiteHeroSection> builder)
    {
        builder
            .Property(e => e.Label)
            .HasWebsiteHeroSectionLabelConfiguration();

        builder
           .Property(e => e.DisplayType)
           .HasWebsiteHeroSectionDisplayTypeConfiguration();

        builder
            .HasMany(e => e.Photos)
            .WithMany(e => e.WebsiteHeroSections)
            .UsingEntity<WebsiteHeroSectionItem>();
    }
}
