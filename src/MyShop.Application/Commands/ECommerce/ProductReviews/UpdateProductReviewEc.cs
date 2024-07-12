using MyShop.Application.Dtos.ECommerce.ProductReviews;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.ProductReviews;

namespace MyShop.Application.Commands.ECommerce.ProductReviews;
public sealed record UpdateProductReviewEc(
    Guid Id,
    int Rate,
    string? Review
    ) : ICommand<ApiResponse<ProductReviewMeEcDto>>,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        ProductReviewRate.Validate(Rate, validationMessages);
        ProductReviewText.Validate(Review, validationMessages);
    }
}
