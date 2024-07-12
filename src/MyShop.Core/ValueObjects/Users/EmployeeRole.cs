using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;

namespace MyShop.Core.ValueObjects.Users;
public sealed record EmployeeRole : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues { get; }
        = new string[]
        {
            SuperAdmin,
            Admin,
            Manager,
            Seller
        }.AsReadOnly();

    public string Value { get; }

    public const string SuperAdmin = nameof(SuperAdmin);
    public const string Admin = nameof(Admin);
    public const string Manager = nameof(Manager);
    public const string Seller = nameof(Seller);

    public EmployeeRole(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<EmployeeRole>());

        Value = value;
    }

    public static implicit operator string(EmployeeRole value)
        => value.Value;

    public static implicit operator EmployeeRole(string value)
        => new(value);

    public override string ToString()
        => Value;
}
