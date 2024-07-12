namespace MyShop.Core.Dtos.Auth;
public sealed record EmployeeMeDto : CustomerMeDto
{
    public required string EmployeeRole { get; init; }
}
