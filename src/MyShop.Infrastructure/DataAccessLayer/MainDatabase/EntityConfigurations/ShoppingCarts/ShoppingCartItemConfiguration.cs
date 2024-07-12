using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.ShoppingCarts;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.ShoppingCarts;
internal sealed class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
{
    public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Quantity)
            .HasShoppingCartItemQuantityConfiguration();

        builder
            .HasOne(e => e.ShoppingCart)
            .WithMany(e => e.ShoppingCartItems)
            .HasForeignKey(e => e.ShoppingCartId)
            .IsRequired();

        builder
            .HasOne(e => e.ProductVariant)
            .WithMany(e => e.ShoppingCartItems)
            .HasForeignKey(e => e.ProductVariantId);
    }
}
