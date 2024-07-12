using Microsoft.OpenApi.Models;
using MyShop.Application.Queries.ECommerce.Products;

namespace MyShop.Infrastructure.Swagger.Operations.ECommerce;
public sealed class GetProductNamesEcOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation.ForParameter(nameof(GetPagedProductsNamesEc.Take))
            .TransformNameToCamelCase()
            .InitSchema()
            .SetAllowedValues([5, 10, 15]);

        operation.ForParameter(nameof(GetPagedProductsNamesEc.SearchPhrase))
            .TransformNameToCamelCase()
            .SetAsRequired()
            .InitSchema()
            .SetType("string");

        return operation;
    }
}
