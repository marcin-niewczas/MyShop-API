using MyShop.Application.Dtos.ECommerce.ProductReviews;
using MyShop.Application.Responses;

namespace MyShop.Application.Queries.ECommerce.Products;
public sealed record GetProductReviewMeByProductIdEc(
    Guid Id
    ) : IQuery<ApiResponse<ProductReviewMeEcDto>>;
