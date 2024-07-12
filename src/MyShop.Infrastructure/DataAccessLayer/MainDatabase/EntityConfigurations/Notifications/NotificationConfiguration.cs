using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Notifications;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Notifications;
internal sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Message)
            .IsRequired();

        builder
            .Property(e => e.ResourceId)
            .IsRequired(false);

        builder
            .Property(e => e.NotificationType)
            .HasNotificationTypeConfiguration();

        builder
            .HasMany(e => e.RegisteredUsers)
            .WithMany(e => e.Notifications)
            .UsingEntity<NotificationRegisteredUser>();
    }
}
