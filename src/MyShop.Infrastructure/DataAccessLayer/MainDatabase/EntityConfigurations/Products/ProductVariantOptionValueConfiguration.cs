using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Products;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Products;
internal sealed class ProductVariantOptionValueConfiguration : IEntityTypeConfiguration<ProductVariantOptionValue>
{
    public void Configure(EntityTypeBuilder<ProductVariantOptionValue> builder)
    {
        builder
            .HasIndex(e => new { e.Value, e.ProductOptionId, e.Position })
            .IsUnique();

        builder
            .HasOne(e => e.ProductVariantOption)
            .WithMany(e => e.ProductOptionValues)
            .HasForeignKey(e => e.ProductOptionId)
            .IsRequired();
    }
}
