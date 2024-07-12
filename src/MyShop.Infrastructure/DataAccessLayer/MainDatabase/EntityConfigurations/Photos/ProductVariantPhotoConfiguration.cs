using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Photos;
using MyShop.Core.Models.Products;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Photos;
internal class ProductVariantPhotoConfiguration : IEntityTypeConfiguration<ProductVariantPhoto>
{
    public void Configure(EntityTypeBuilder<ProductVariantPhoto> builder)
    {
        builder
            .HasMany(e => e.ProductVariants)
            .WithMany(e => e.Photos)
            .UsingEntity<ProductVariantPhotoItem>();
    }
}
