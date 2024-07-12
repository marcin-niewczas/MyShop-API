using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Products;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Products;
internal sealed class BaseProductOptionValueConfiguration : IEntityTypeConfiguration<BaseProductOptionValue>
{
    public void Configure(EntityTypeBuilder<BaseProductOptionValue> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Value)
            .HasProductOptionValueConfiguration();

        builder
           .Property(e => e.Position)
           .HasProductOptionPositionConfiguration();

        builder
            .Property(e => e.Discriminator)
            .HasProductOptionTypeConfiguration();

        builder
            .HasDiscriminator(e => e.Discriminator)
            .HasValue<ProductVariantOptionValue>(ProductOptionType.Variant)
            .HasValue<ProductDetailOptionValue>(ProductOptionType.Detail);

        builder.ToTable(nameof(BaseProductOption.ProductOptionValues));
    }
}
