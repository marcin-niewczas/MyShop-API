using Microsoft.OpenApi.Models;
using MyShop.Application.EndpointQueries.ProductOptions;
using MyShop.Application.Queries.ECommerce.Categories;

namespace MyShop.Infrastructure.Swagger.Operations.ECommerce;
public sealed class GetProductFiltersByCategoryIdEcOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation.ForParameter(nameof(GetProductFiltersByCategoryIdEc.MinPrice))
                 .TransformNameToCamelCase()
                 .InitSchema()
                 .SetType("number")
                 .SetFormat("double")
                 .SetMinimum(0);

        operation.ForParameter(nameof(GetProductFiltersByCategoryIdEc.MaxPrice))
                 .TransformNameToCamelCase()
                 .InitSchema()
                 .SetType("number")
                 .SetFormat("double")
                 .SetMinimum(0);

        operation.ForParameter(nameof(GetProductFiltersByCategoryIdEc.ProductOptionParam))
                 .TransformNameToCamelCase()
                 .SetDescription(ProductOptionParam.GetSwaggerDescription())
                 .InitSchema()
                 .SetType("string"); ;

        return operation;
    }
}
