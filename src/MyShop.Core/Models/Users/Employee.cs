using MyShop.Core.ValueObjects.Shared;
using MyShop.Core.ValueObjects.Users;

namespace MyShop.Core.Models.Users;
public sealed class Employee : RegisteredUser
{
    public EmployeeRole EmployeeRole { get; private set; }

    public Employee(
        FirstName firstName,
        LastName lastName,
        Email email,
        Gender gender,
        EmployeeRole employeeRole,
        string securedPassword,
        DateOfBirth dateOfBirth,
        UserPhoneNumber phoneNumber
        ) : base(
            UserRole.Employee,
            firstName,
            lastName,
            email,
            gender,
            securedPassword,
            dateOfBirth,
            phoneNumber
            )
    {
        EmployeeRole = employeeRole ?? throw new ArgumentNullException(nameof(employeeRole));
    }
}
