using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Products;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Products;
internal sealed class ProductProductVariantOptionConfiguration : IEntityTypeConfiguration<ProductProductVariantOption>
{
    public void Configure(EntityTypeBuilder<ProductProductVariantOption> builder)
    {
        builder
            .Property(e => e.Position)
            .HasProductOptionPositionConfiguration();
    }
}
