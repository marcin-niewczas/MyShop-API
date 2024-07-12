namespace MyShop.Infrastructure.Options;
internal sealed record MyShopPayOptions
{
    public const string Section = nameof(MyShopPayOptions);

    public Uri Uri { get; init; } = default!;
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
}
