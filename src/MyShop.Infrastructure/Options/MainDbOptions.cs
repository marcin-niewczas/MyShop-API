namespace MyShop.Infrastructure.Options;
internal sealed class MainDbOptions
{
    public const string Section = nameof(MainDbOptions);
    public required string ConnectionString { get; init; }
}
