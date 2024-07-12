namespace MyShop.Core.Abstractions;
public abstract record BaseDto : IDto
{
    public required Guid Id { get; init; }
}

public abstract record BaseTimestampDto : BaseDto
{
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset? UpdatedAt { get; init; }
}

public interface IDto
{
}
