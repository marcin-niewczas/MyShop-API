using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Products;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Products;
internal sealed class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder
           .HasKey(e => e.Id);

        builder
            .HasIndex(e => new { e.RegisteredUserId, e.EncodedProductVariantName })
            .IsUnique();

        builder
            .HasOne(e => e.RegisteredUser)
            .WithMany(e => e.Favorites)
            .HasForeignKey(e => e.RegisteredUserId);
    }
}
