using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Orders;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Orders;
internal sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.InvoiceNumber)
            .IsRequired();
    }
}
