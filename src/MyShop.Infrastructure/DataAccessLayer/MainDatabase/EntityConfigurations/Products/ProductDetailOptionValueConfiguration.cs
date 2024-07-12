using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Products;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Products;
internal sealed class ProductDetailOptionValueConfiguration : IEntityTypeConfiguration<ProductDetailOptionValue>
{
    public void Configure(EntityTypeBuilder<ProductDetailOptionValue> builder)
    {
        builder
           .HasIndex(e => new { e.Value, e.ProductOptionId, e.Position })
           .IsUnique();

        builder
            .HasOne(e => e.ProductDetailOption)
            .WithMany(e => e.ProductOptionValues)
            .HasForeignKey(e => e.ProductOptionId)
            .IsRequired();

        builder
            .HasMany(e => e.Products)
            .WithMany(e => e.ProductDetailOptionValues)
            .UsingEntity<ProductProductDetailOptionValue>();
    }
}
