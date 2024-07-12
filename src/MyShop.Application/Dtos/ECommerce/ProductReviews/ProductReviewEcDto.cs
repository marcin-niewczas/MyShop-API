using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.ECommerce.ProductReviews;
public sealed record ProductReviewEcDto : BaseTimestampDto
{
    public required string? Review { get; init; }
    public required int Rate { get; init; }
    public required string UserFirstName { get; init; }
    public required Uri? UserPhotoUrl { get; init; }
}
