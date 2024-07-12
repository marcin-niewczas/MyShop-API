using Microsoft.EntityFrameworkCore;
using MyShop.Core.Models.MainPageSections;
using MyShop.Core.Models.Notifications;
using MyShop.Core.Models.Orders;
using MyShop.Core.Models.Photos;
using MyShop.Core.Models.Products;
using MyShop.Core.Models.ShoppingCarts;
using MyShop.Core.Models.Users;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase;
internal sealed class MainDbContext(
    DbContextOptions<MainDbContext> options
    ) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationRegisteredUser> NotificationRegisteredUsers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductDetailOption> ProductDetailOptions { get; set; }
    public DbSet<ProductDetailOptionValue> ProductDetailOptionValues { get; set; }
    public DbSet<ProductProductDetailOptionValue> ProductProductDetailOptionValues { get; set; }
    public DbSet<ProductReview> ProductReviews { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<ProductVariantOption> ProductVariantOptions { get; set; }
    public DbSet<ProductProductVariantOption> ProductProductVariantOptions { get; set; }
    public DbSet<ProductVariantOptionValue> ProductVariantOptionValues { get; set; }
    public DbSet<ProductVariantPhoto> ProductVariantPhotos { get; set; }
    public DbSet<ProductVariantPhotoItem> ProductVariantPhotoItems { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    public DbSet<UserAddress> UserAddresses { get; set; }
    public DbSet<UserPhoto> UserPhotos { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }
    public DbSet<WebsiteProductsCarouselSection> WebsiteProductCarouselSections { get; set; }
    public DbSet<WebsiteHeroSection> WebsiteHeroSections { get; set; }
    public DbSet<WebsiteHeroSectionItem> WebsiteHeroSectionItems { get; set; }
    public DbSet<WebsiteHeroSectionPhoto> WebsiteSectionPhotos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
