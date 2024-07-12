using Microsoft.OpenApi.Models;
using MyShop.Core.RepositoryQueryParams.Account;

namespace MyShop.Infrastructure.Swagger.Operations.Account;
public sealed class GetPagedFavoritesAcOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation.SetPaginationParameters();
        operation.SetSortParameters<GetPagedFavoritesAcSortBy>();
        operation.SetSearchPhraseParameter();

        return operation;
    }
}
