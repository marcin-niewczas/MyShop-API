using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Users;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Users;
internal sealed class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Browser)
            .HasEnumConversion()
            .IsRequired();

        builder
            .Property(e => e.OperatingSystem)
            .HasEnumConversion()
            .IsRequired();

        builder
            .Property(e => e.IsMobile)
            .IsRequired();

        builder
            .Property(e => e.RefreshToken)
            .IsRequired();

        builder
            .Property(e => e.ExpiryRefreshTokenDate)
            .IsRequired();

        builder
            .HasOne(e => e.User)
            .WithMany(e => e.UserTokens)
            .HasForeignKey(e => e.UserId)
            .IsRequired();
    }
}
