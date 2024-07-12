using Microsoft.OpenApi.Models;
using MyShop.Application.EndpointQueries;
using MyShop.Application.Queries.ECommerce.Favorites;

namespace MyShop.Infrastructure.Swagger.Operations.ECommerce;
public sealed class GetStatusOfFavoritesEcOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation.ForParameter(nameof(GetStatusOfFavoritesEc.ProductEncodedNames))
            .TransformNameToCamelCase()
            .SetDescription(StringCollectionQueryParam.GetSwaggerDescription())
            .InitSchema()
            .SetType("string");

        return operation;
    }
}
