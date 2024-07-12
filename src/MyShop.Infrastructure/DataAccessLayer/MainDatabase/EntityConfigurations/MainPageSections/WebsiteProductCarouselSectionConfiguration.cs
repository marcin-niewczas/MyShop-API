using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.MainPageSections;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.MainPageSections;
internal sealed class WebsiteProductCarouselSectionConfiguration : IEntityTypeConfiguration<WebsiteProductsCarouselSection>
{
    public void Configure(EntityTypeBuilder<WebsiteProductsCarouselSection> builder)
    {
        builder
            .Property(e => e.ProductsCarouselSectionType)
            .HasProductCarouselSectionTypeConfiguration();
    }
}
