using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.Users;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations.Users;
internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Role)
            .HasUserRoleConfiguration();

        builder
            .HasDiscriminator(e => e.Role)
            .HasValue<Customer>(UserRole.Customer)
            .HasValue<Employee>(UserRole.Employee)
            .HasValue<Guest>(UserRole.Guest);

        builder.ToTable("Users");
    }
}
