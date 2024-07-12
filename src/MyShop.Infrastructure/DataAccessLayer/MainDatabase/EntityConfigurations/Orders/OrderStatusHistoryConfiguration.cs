using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Orders;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Orders;
internal sealed class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
{
    public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Status)
            .HasOrderStatusConfiguration();
    }
}
