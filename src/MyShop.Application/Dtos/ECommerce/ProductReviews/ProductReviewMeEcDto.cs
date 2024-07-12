using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.ECommerce.ProductReviews;
public sealed record ProductReviewMeEcDto : BaseTimestampDto
{
    public required string? Review { get; init; }
    public required int Rate { get; init; }
}
