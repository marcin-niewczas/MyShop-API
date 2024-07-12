using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Users;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Users;
internal sealed class RegisteredUserConfiguration : IEntityTypeConfiguration<RegisteredUser>
{
    public void Configure(EntityTypeBuilder<RegisteredUser> builder)
    {
        builder
            .Property(e => e.Email)
            .HasEmailConfiguration();

        builder
            .HasIndex(e => e.Email)
            .IsUnique();

        builder
            .Property(e => e.FirstName)
            .HasFirstNameConfiguration();

        builder
            .Property(e => e.LastName)
            .HasLastNameConfiguration();

        builder
            .Property(e => e.SecuredPassword)
            .IsRequired();

        builder
            .Property(e => e.Gender)
            .HasGenderConfiguration();

        builder
            .Property(e => e.PhoneNumber)
            .HasUserPhoneNumberConfiguration();

        builder
            .Property(e => e.DateOfBirth)
            .HasDateOfBirthConfiguration();
    }
}
