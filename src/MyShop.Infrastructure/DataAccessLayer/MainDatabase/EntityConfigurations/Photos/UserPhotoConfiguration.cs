using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Photos;
using MyShop.Core.Models.Users;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Photos;
internal sealed class UserPhotoConfiguration : IEntityTypeConfiguration<UserPhoto>
{
    public void Configure(EntityTypeBuilder<UserPhoto> builder)
    {
        builder
            .HasOne(e => e.RegisteredUser)
            .WithOne(e => e.Photo)
            .HasForeignKey<RegisteredUser>(e => e.PhotoId);
    }
}
