using MyShop.Application.Responses;
using MyShop.Core.Dtos.ECommerce;

namespace MyShop.Application.Queries.ECommerce.Products;
public sealed record GetProductReviewRateStatsEc(
    Guid Id
    ) : IQuery<ApiResponse<ProductReviewRateStatEcDto>>;
