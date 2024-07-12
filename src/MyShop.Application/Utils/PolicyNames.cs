namespace MyShop.Application.Utils;
public readonly record struct PolicyNames
{
    public const string HasGuestPermission = "has-guest-permission";
    public const string HasCustomerPermission = "has-customer-permission";
    public const string HasEmployeePermission = "has-employee-permission";

    public const string HasManagerPermission = "has-manager-permission";
    public const string HasAdminPermission = "has-admin-permission";
    public const string HasSuperAdminPermission = "has-super-admin-permission";
}
