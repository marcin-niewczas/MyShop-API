using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.ManagementPanel.ProductReviews;
public sealed record ProductReviewMpDto : BaseTimestampDto
{
    public required string? Review { get; init; }
    public required int Rate { get; init; }
    public required string UserFirstName { get; init; }
    public required string UserLastName { get; init; }
    public required Uri? UserPhotoUrl { get; init; }
}
