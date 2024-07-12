namespace MyShop.Infrastructure.Options;
public sealed class AppOptions
{
    public const string Section = nameof(AppOptions);
    public required string Name { get; init; }
}
