using MyShop.Core.Abstractions;
using MyShop.Core.HelperModels;

namespace MyShop.Application.Dtos.Account.Users;
public sealed record UserActiveDeviceAcDto : IDto
{
    public required string BrowserFullName { get; init; }
    public required string? BrowserVersion { get; init; }
    public required OS OperatingSystem { get; init; }
    public required bool IsMobile { get; init; }
    public required DateTimeOffset LastActivity { get; init; }
    public required bool IsCurrentDevice { get; init; }
}
