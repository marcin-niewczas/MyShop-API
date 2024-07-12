using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.ShoppingCarts;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.ShoppingCarts;
internal sealed class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .HasMany(e => e.ShoppingCartItems)
            .WithOne(e => e.ShoppingCart)
            .HasForeignKey(e => e.ShoppingCartId)
            .IsRequired();

        builder
            .HasOne(e => e.User)
            .WithOne(e => e.ShoppingCart)
            .HasForeignKey<ShoppingCart>(e => e.UserId)
            .IsRequired();
    }
}
