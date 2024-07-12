using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Orders;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Orders;
internal sealed class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Quantity)
            .HasOrderProductQuantityConfiguration();

        builder
            .Property(e => e.Price)
            .HasOrderProductPriceConfiguration();

        builder
            .HasOne(e => e.ProductVariant)
            .WithMany(e => e.OrderProducts)
            .HasForeignKey(e => e.ProductVariantId);

        builder
            .HasOne(e => e.Order)
            .WithMany(e => e.OrderProducts)
            .HasForeignKey(e => e.OrderId);
    }
}
