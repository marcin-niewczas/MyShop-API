using MyShop.Core.Abstractions;

namespace MyShop.Core.Dtos.ECommerce;
public sealed record ProductReviewRateStatEcDto : IDto
{
    public required int ProductReviewsCount { get; init; }
    public required int SumProductReviews { get; init; }
    public double AvarageProductReviews => ProductReviewsCount == 0 ? 0 : Math.Round((double)SumProductReviews / ProductReviewsCount, 1);
    public required IReadOnlyCollection<RateCountEcDto> RateCounts { get; init; }
}

public sealed record RateCountEcDto : IDto
{
    public required int Rate { get; init; }
    public required int Count { get; init; }
}
