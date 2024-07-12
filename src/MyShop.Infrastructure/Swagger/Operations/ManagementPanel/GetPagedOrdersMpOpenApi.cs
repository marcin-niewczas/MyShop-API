using Microsoft.OpenApi.Models;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;

namespace MyShop.Infrastructure.Swagger.Operations.ManagementPanel;
public sealed class GetPagedOrdersMpOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation.SetPaginationParameters();
        operation.SetSortParameters<GetPagedOrdersMpSortBy>();
        operation.SetSearchPhraseParameter();
        operation.SetTimestampParameters();

        return operation;
    }
}
