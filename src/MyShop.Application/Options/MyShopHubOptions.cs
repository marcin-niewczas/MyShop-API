namespace MyShop.Application.Options;
public sealed record MyShopHubOptions
{
    public const string Section = nameof(MyShopHubOptions);
    public string SharedPath { get; init; } = default!;
}
