using Microsoft.OpenApi.Models;
using MyShop.Application.Queries.ECommerce.Products;
using MyShop.Core.RepositoryQueryParams.Shared;
using MyShop.Core.ValueObjects.ProductReviews;

namespace MyShop.Infrastructure.Swagger.Operations.ECommerce;
public sealed class GetPagedProductReviewsEcOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation.SetPaginationParameters();
        operation.SetSortParameters<GetPagedProductReviewsSortBy>();
        operation.ForParameter(nameof(GetPagedProductReviewsEc.ProductReviewRate))
                .TransformNameToCamelCase()
                .InitSchema()
                .SetAllowedValues(Enumerable.Range(ProductReviewRate.Min, ProductReviewRate.Max));

        return operation;
    }
}
