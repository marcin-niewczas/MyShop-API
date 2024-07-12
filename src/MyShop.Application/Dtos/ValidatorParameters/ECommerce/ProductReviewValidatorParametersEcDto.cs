using MyShop.Core.Abstractions;
using MyShop.Core.ValueObjects.ProductReviews;

namespace MyShop.Application.Dtos.ValidatorParameters.ECommerce;
public sealed record ProductReviewValidatorParametersEcDto : IDto
{
    public StringValidatorParameters ProductReviewTextParams { get; } = new()
    {
        MaxLength = ProductReviewText.MaxLength,
        IsRequired = false
    };
    public int MinProductReviewRate { get; }
        = ProductReviewRate.Min;
    public int MaxProductReviewRate { get; }
        = ProductReviewRate.Max;
}
