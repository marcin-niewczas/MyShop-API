using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Products;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Products;
internal sealed class ProductVariantPhotoItemConfiguration : IEntityTypeConfiguration<ProductVariantPhotoItem>
{
    public void Configure(EntityTypeBuilder<ProductVariantPhotoItem> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Position)
            .HasProductVariantPhotoItemPositionConfiguration();
    }
}
