using Microsoft.OpenApi.Models;
using MyShop.Application.Queries.ManagementPanel.Categories;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;

namespace MyShop.Infrastructure.Swagger.Operations.ManagementPanel;
public sealed class GetCategoryMpOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation.ForParameter(nameof(GetCategoryMp.QueryType))
                    .TransformNameToCamelCase()
                    .InitSchema()
                    .SetEnumValues<GetCategoryMpQueryType>();

        return operation;
    }
}
