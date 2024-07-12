using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.ManagementPanel.Users;
public record CustomerMpDto : BaseTimestampDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
}

public sealed record EmployeeMpDto : CustomerMpDto
{
    public required string EmployeeRole { get; init; }
}
