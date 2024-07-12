namespace MyShop.Application.Commands.ECommerce.ProductReviews;
public sealed record RemoveProductReviewEc(
    Guid ProductReviewId
    ) : ICommand;
