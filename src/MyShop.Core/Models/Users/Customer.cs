using MyShop.Core.ValueObjects.Shared;
using MyShop.Core.ValueObjects.Users;

namespace MyShop.Core.Models.Users;
public sealed class Customer : RegisteredUser
{
    public Customer(
        FirstName firstName,
        LastName lastName,
        Email email,
        Gender gender,
        string securedPassword,
        DateOfBirth dateOfBirth,
        UserPhoneNumber phoneNumber
        ) : base(
                UserRole.Customer,
                firstName,
                lastName,
                email,
                gender,
                securedPassword,
                dateOfBirth,
                phoneNumber
            )
    {
    }
}
