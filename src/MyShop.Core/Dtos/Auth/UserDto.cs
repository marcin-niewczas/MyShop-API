using MyShop.Core.Abstractions;
using System.Text.Json.Serialization;

namespace MyShop.Core.Dtos.Auth;
[JsonDerivedType(typeof(CustomerMeDto))]
[JsonDerivedType(typeof(EmployeeMeDto))]
public record UserDto : BaseDto
{
    public required string UserRole { get; init; }
}
