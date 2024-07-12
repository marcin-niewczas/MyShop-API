using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Orders;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Orders;
internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Email)
            .HasEmailConfiguration();

        builder
            .Property(e => e.FirstName)
            .HasFirstNameConfiguration();

        builder
            .Property(e => e.LastName)
            .HasLastNameConfiguration();

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
            .Property(e => e.PhoneNumber)
            .HasOrderPhoneNumberConfiguration();

        builder
            .HasOne(e => e.User)
            .WithMany(e => e.Orders)
            .HasForeignKey(e => e.UserId);

        builder
            .HasMany(e => e.OrderStatusHistories)
            .WithOne(e => e.Order)
            .HasForeignKey(e => e.OrderId);

        builder
            .Property(e => e.PaymentId)
            .IsRequired(false);

        builder
            .Property(e => e.RedirectPaymentUri)
            .IsRequired(false);

        builder
            .Property(e => e.PaymentMethod)
            .HasPaymentMethodConfiguration();

        builder
            .Property(e => e.DeliveryMethod)
            .HasDeliveryMethodConfiguration();

        builder
            .Property(e => e.Status)
            .HasOrderStatusConfiguration();
    }
}
