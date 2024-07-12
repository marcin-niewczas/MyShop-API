using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Products;
using MyShop.Core.ValueObjects.Products;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Products;
internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Name)
            .HasProductNameConfiguration();

        builder
            .HasIndex(e => e.Name)
            .IsUnique();

        builder
            .Property(e => e.EncodedName)
            .HasMaxLength(ProductName.MaxLength)
            .IsRequired();

        builder
            .Property(e => e.Description)
            .HasProductDescriptionConfiguration();

        builder
            .Property(e => e.DisplayProductType)
            .HasDisplayProductTypeConfiguration();

        builder
            .HasOne(e => e.Category)
            .WithMany(e => e.Products)
            .HasForeignKey(e => e.CategoryId)
            .IsRequired();
    }
}
