using Microsoft.OpenApi.Models;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;

namespace MyShop.Infrastructure.Swagger.Operations.ManagementPanel;
public sealed class GetPagedProductsMpOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation.SetPaginationParameters();
        operation.SetSortParameters<GetPagedProductsMpSortBy>();
        operation.SetSearchPhraseParameter();

        return operation;
    }
}
