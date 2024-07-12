using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Products;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Products;
internal sealed class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Price)
            .HasProductVariantPriceConfiguration();

        builder
            .Property(e => e.SkuId)
            .IsRequired();

        builder
            .Property(e => e.EncodedName)
            .IsRequired();

        builder
            .Property(e => e.Quantity)
            .HasProductVariantItemQuantityConfiguration();

        builder
            .HasOne(e => e.Product)
            .WithMany(e => e.ProductVariants)
            .HasForeignKey(e => e.ProductId)
            .IsRequired();

        builder
            .HasMany(e => e.ProductVariantOptionValues)
            .WithMany(e => e.ProductVariants);
    }
}
