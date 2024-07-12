namespace MyShop.Infrastructure.Options;
public sealed record WebSPAClientOptions
{
    public const string Section = nameof(WebSPAClientOptions);
    public Uri CurrentUri { get; init; } = default!;
    public IReadOnlyCollection<Uri> AllowedOriginUrls { get; init; } = default!;
}
