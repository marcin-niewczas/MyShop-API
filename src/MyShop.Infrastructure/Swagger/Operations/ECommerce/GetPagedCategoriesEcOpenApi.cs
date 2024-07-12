using Microsoft.OpenApi.Models;
using MyShop.Application.Queries.ECommerce.Categories;
using MyShop.Core.RepositoryQueryParams.Shared;

namespace MyShop.Infrastructure.Swagger.Operations.ECommerce;
public sealed class GetPagedCategoriesEcOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation.SetPaginationParameters();
        operation.SetSearchPhraseParameter();

        operation.ForParameter(nameof(GetPagedCategoriesEc.QueryType))
                 .TransformNameToCamelCase()
                 .InitSchema()
                 .SetEnumValues<GetPagedCategoriesQueryType>();

        return operation;
    }
}
