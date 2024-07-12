using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Products;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Products;
internal sealed class ProductProductDetailOptionValueConfiguration : IEntityTypeConfiguration<ProductProductDetailOptionValue>
{
    public void Configure(EntityTypeBuilder<ProductProductDetailOptionValue> builder)
    {
        builder
            .Property(e => e.Position)
            .HasProductOptionPositionConfiguration();
    }
}
