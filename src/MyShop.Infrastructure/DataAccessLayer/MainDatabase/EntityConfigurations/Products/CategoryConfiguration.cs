using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Products;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Products;
internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Name)
            .HasCategoryNameConfiguration();

        builder
            .HasIndex(e => new { e.Name, e.ParentCategoryId })
            .IsUnique()
            .HasFilter(null);

        builder
            .OwnsOne(e => e.HierarchyDetail, nav =>
            {
                nav
                .Property(p => p.HierarchyName)
                .IsRequired();

                nav
                .HasIndex(p => p.HierarchyName)
                .IsUnique();

                nav
                .Property(p => p.Level)
                .HasCategoryLevelConfiguration();

                nav
                .Property(p => p.EncodedHierarchyName)
                .IsRequired();

                nav
                .HasIndex(p => p.EncodedHierarchyName)
                .IsUnique();
            });

        builder
            .HasMany(e => e.ChildCategories)
            .WithOne(e => e.ParentCategory)
            .HasForeignKey(e => e.ParentCategoryId)
            .IsRequired(false);
    }
}
