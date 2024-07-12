using MyShop.Core.Abstractions;

namespace MyShop.Core.Dtos.Shared;
public sealed record OrderStatusHistoryDto : BaseTimestampDto
{
    public required string Status { get; init; }
}
