using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Products;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Products;
internal sealed class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
{
    public void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Review)
            .HasProductReviewTextConfiguration();

        builder
            .Property(e => e.Rate)
            .HasProductReviewRateConfiguration();

        builder
            .HasOne(e => e.RegisteredUser)
            .WithMany(e => e.ProductReviews)
            .HasForeignKey(e => e.RegisteredUserId);

        builder
            .HasOne(e => e.Product)
            .WithMany(e => e.ProductReviews)
            .HasForeignKey(e => e.ProductId);
    }
}
