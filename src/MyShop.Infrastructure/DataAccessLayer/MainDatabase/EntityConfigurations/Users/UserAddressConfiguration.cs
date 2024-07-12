using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyShop.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Users;
internal sealed class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
{
    public void Configure(EntityTypeBuilder<UserAddress> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.StreetName)
            .HasStreetNameConfiguration();

        builder
            .Property(e => e.BuildingNumber)
            .HasBuildingNumberConfiguration();

        builder
           .Property(e => e.ApartmentNumber)
           .HasApartmentNumberConfiguration();

        builder
            .Property(e => e.City)
            .HasCityConfiguration();

        builder
            .Property(e => e.ZipCode)
            .HasZipCodeConfiguration();

        builder
            .Property(e => e.Country)
            .HasCountryConfiguration();

        builder
            .Property(e => e.UserAddressName)
            .HasUserAddressNameConfiguration();

        builder
            .HasIndex(e => new { e.RegisteredUserId, e.UserAddressName })
            .IsUnique();

        builder
            .HasOne(e => e.RegisteredUser)
            .WithMany(e => e.UserAddresses)
            .HasForeignKey(e => e.RegisteredUserId);
    }
}
