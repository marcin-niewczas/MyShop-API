namespace MyShop.Infrastructure.Options;
public sealed class MessagingOptions
{
    public const string Section = nameof(MessagingOptions);
    public bool UseRabbitMQ { get; init; } = default!;
    public string Hostname { get; init; } = default!;
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
}
