using Microsoft.OpenApi.Models;
using MyShop.Application.Queries.ManagementPanel.Categories;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;
using MyShop.Core.RepositoryQueryParams.Shared;

namespace MyShop.Infrastructure.Swagger.Operations.ManagementPanel;
public sealed class GetPagedCategoriesMpOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation.SetPaginationParameters();
        operation.SetSortParameters<GetPagedCategoriesMpSortBy>();
        operation.SetSearchPhraseParameter();

        operation.ForParameter(nameof(GetPagedCategoriesMp.QueryType))
            .TransformNameToCamelCase()
            .InitSchema()
            .SetEnumValues<GetPagedCategoriesQueryType>();

        return operation;
    }
}
