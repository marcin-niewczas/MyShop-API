namespace MyShop.Core.Dtos.Auth;
public record CustomerMeDto : UserDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string Gender { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required string? PhoneNumber { get; init; }
    public required string? PhotoUrl { get; init; }
}
