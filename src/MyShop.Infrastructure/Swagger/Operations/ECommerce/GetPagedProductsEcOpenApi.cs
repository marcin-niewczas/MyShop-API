using Microsoft.OpenApi.Models;
using MyShop.Application.EndpointQueries.ProductOptions;
using MyShop.Application.Queries.ECommerce.Products;
using MyShop.Core.RepositoryQueryParams.ECommerce;

namespace MyShop.Infrastructure.Swagger.Operations.ECommerce;
public sealed class GetPagedProductsEcOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation.SetPaginationParameters();
        operation.SetSearchPhraseParameter();
        operation.SetSortByParameter<GetPagedProductsEcSortBy>();

        operation.ForParameter(nameof(GetPagedProductsEc.ProductOptionParam))
                 .TransformNameToCamelCase()
                 .SetDescription(ProductOptionParam.GetSwaggerDescription())
                 .InitSchema()
                 .SetType("string");

        operation.ForParameter(nameof(GetPagedProductsEc.MinPrice))
                 .TransformNameToCamelCase()
                 .InitSchema()
                 .SetType("number")
                 .SetFormat("double")
                 .SetMinimum(0);

        operation.ForParameter(nameof(GetPagedProductsEc.MaxPrice))
                 .TransformNameToCamelCase()
                 .InitSchema()
                 .SetType("number")
                 .SetFormat("double")
                 .SetMinimum(0);

        operation.ForParameter(nameof(GetPagedProductsEc.EncodedCategoryName))
                 .TransformNameToCamelCase()
                 .InitSchema()
                 .SetType("string");

        return operation;
    }
}
