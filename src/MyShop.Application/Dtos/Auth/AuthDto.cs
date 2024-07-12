using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.Auth;
public sealed record AuthDto : IDto
{
    public required string AccessToken { get; init; }
    public required DateTimeOffset ExpiryAccessTokenDate { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTimeOffset ExpiryRefreshTokenDate { get; init; }
    public required Guid UserTokenId { get; init; }
    public required string UserRole { get; init; }
}
