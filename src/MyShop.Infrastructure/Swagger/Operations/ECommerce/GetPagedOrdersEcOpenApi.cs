using Microsoft.OpenApi.Models;
using MyShop.Core.RepositoryQueryParams.ECommerce;

namespace MyShop.Infrastructure.Swagger.Operations.ECommerce;
public sealed class GetPagedOrdersEcOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation.SetPaginationParameters();
        operation.SetSortParameters<GetPagedOrdersEcSortBy>();
        operation.SetSearchPhraseParameter();
        operation.SetTimestampParameters();

        return operation;
    }
}
