using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Products;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Products;
internal sealed class ProductDetailOptionConfiguration : IEntityTypeConfiguration<ProductDetailOption>
{
    public void Configure(EntityTypeBuilder<ProductDetailOption> builder)
    {

    }
}
