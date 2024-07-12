using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Notifications;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Notifications;
internal sealed class NotificationRegisteredUserConfiguration : IEntityTypeConfiguration<NotificationRegisteredUser>
{
    public void Configure(EntityTypeBuilder<NotificationRegisteredUser> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.IsRead)
            .IsRequired();
    }
}
