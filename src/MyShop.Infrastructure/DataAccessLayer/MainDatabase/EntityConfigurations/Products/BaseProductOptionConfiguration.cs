using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Products;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Products;
internal sealed class BaseProductOptionConfiguration : IEntityTypeConfiguration<BaseProductOption>
{
    public void Configure(EntityTypeBuilder<BaseProductOption> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.ProductOptionSubtype)
            .HasProductOptionSubtypeConfiguration();

        builder
            .Property(e => e.ProductOptionSortType)
            .HasProductOptionSortTypeConfiguration();

        builder
            .Property(e => e.Name)
            .HasProductOptionNameConfiguration();

        builder
            .HasIndex(e => e.Name)
            .IsUnique();

        builder
            .Property(e => e.ProductOptionType)
            .HasProductOptionTypeConfiguration();

        builder
            .HasDiscriminator(e => e.ProductOptionType)
            .HasValue<ProductVariantOption>(ProductOptionType.Variant)
            .HasValue<ProductDetailOption>(ProductOptionType.Detail);

        builder.ToTable("ProductOptions");
    }
}
