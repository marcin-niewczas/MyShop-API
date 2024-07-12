using MyShop.Core.ValueObjects.Users;

namespace MyShop.Core.HelperModels;
public record UserClaimsData
{
    public required Guid UserId { get; init; }
    public required UserRole UserRole { get; init; }
    public required Guid UserTokenId { get; init; }
}

public record CustomerClaimsData : UserClaimsData
{
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required Gender Gender { get; init; }
    public required DateOnly DateOfBirth { get; init; }
}

public record EmployeeClaimsData : CustomerClaimsData
{
    public required EmployeeRole EmployeeRole { get; init; }
}
