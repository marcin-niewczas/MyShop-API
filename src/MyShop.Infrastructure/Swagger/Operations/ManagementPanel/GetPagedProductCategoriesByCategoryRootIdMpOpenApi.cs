using Microsoft.OpenApi.Models;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;

namespace MyShop.Infrastructure.Swagger.Operations.ManagementPanel;
public sealed class GetPagedProductCategoriesByCategoryRootIdMpOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation.SetPaginationParameters();
        operation.SetSortParameters<GetPagedCategoriesMpSortBy>();
        operation.SetSearchPhraseParameter();

        return operation;
    }
}
