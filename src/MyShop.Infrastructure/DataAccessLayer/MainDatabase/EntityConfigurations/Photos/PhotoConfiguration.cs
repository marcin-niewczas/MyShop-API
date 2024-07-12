using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyShop.Core.Models.Photos;
using MyShop.Core.ValueObjects.Photos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Photos;
internal sealed class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Name)
            .HasPhotoNameConfiguration();

        builder
            .HasIndex(e => e.Name)
            .IsUnique();

        builder
            .Property(e => e.Alt)
            .HasPhotoAltConfiguration();

        builder
            .Property(e => e.FilePath)
            .HasPhotoFilePathConfiguration();

        builder
            .HasIndex(e => e.FilePath)
            .IsUnique();

        builder
            .Property(e => e.ContentType)
            .HasPhotoContentTypeConfiguration();

        builder
           .Property(e => e.Extension)
           .HasPhotoExtensionConfiguration();

        builder
            .Property(e => e.Uri)
            .HasConversion(v => v.ToString(), v => new Uri(v))
            .IsRequired();

        builder
            .HasIndex(e => e.Uri)
            .IsUnique();

        builder
            .Property(e => e.PhotoType)
            .HasPhotoTypeConfiguration();

        builder
            .Property(e => e.PhotoSize)
            .HasPhotoSizeConfiguration();

        builder
            .HasDiscriminator(e => e.PhotoType)
            .HasValue<ProductVariantPhoto>(PhotoType.ProductVariantPhoto)
            .HasValue<UserPhoto>(PhotoType.UserPhoto)
            .HasValue<WebsiteHeroSectionPhoto>(PhotoType.WebsiteHeroPhoto);

        builder
            .ToTable("Photos");
    }
}
