using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Core.ValueObjects.Users;
public sealed record UserRole : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues { get; }
        = new string[] {
            Customer,
            Employee,
            Guest,
        }.AsReadOnly();
    public string Value { get; }

    public const string Customer = nameof(Customer);
    public const string Employee = nameof(Employee);
    public const string Guest = nameof(Guest);

    public bool HasGuestPermission()
        => HasGuestPermission(Value);

    public bool HasCustomerPermission()
        => HasCustomerPermission(Value);

    public bool HasEmployeePermission()
        => HasEmployeePermission(Value);

    public static bool HasGuestPermission(string role)
        => role is Employee or Customer or Guest;

    public static bool HasCustomerPermission(string role)
        => role is Employee or Customer;

    public static bool HasEmployeePermission(string role)
        => role == Employee;

    public UserRole(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<UserRole>());

        Value = value;
    }

    public static implicit operator string(UserRole value)
        => value.Value;

    public static implicit operator UserRole(string value)
        => new(value);

    public override string ToString()
        => Value;
}
