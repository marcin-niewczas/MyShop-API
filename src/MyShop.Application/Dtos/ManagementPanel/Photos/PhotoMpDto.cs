using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.ManagementPanel.Photos;
public sealed record PhotoMpDto : BaseTimestampDto
{
    public required string Name { get; init; }
    public required string PhotoExtension { get; init; }
    public required string ContentType { get; init; }
    public required decimal PhotoSize { get; init; }
    public required string Url { get; init; }
    public required string Alt { get; init; }
    public required string PhotoType { get; init; }
}
