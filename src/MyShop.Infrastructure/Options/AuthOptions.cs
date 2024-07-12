namespace MyShop.Infrastructure.Options;
internal sealed class AuthOptions
{
    public const string Section = nameof(AuthOptions);

    public string Issuer { get; init; } = default!;
    public string Audience { get; init; } = default!;
    public string SigningKey { get; init; } = default!;
    public TimeSpan? ExpiryAccessToken { get; init; }
    public TimeSpan? ExpiryRefreshToken { get; init; }
}
