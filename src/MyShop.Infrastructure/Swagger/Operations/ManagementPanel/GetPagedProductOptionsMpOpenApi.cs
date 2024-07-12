using Microsoft.OpenApi.Models;
using MyShop.Application.Queries.ManagementPanel.ProductOptions;
using MyShop.Core.RepositoryQueryParams.ManagementPanel;

namespace MyShop.Infrastructure.Swagger.Operations.ManagementPanel;
public sealed class GetPagedProductOptionsMpOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation.SetPaginationParameters();
        operation.SetSortParameters<GetPagedProductOptionsMpSortBy>();
        operation.SetSearchPhraseParameter();

        operation.ForParameter(nameof(GetPagedProductOptionsMp.QueryType))
            .TransformNameToCamelCase()
            .InitSchema()
            .SetEnumValues<ProductOptionTypeMpQueryType>();

        operation.ForParameter(nameof(GetPagedProductOptionsMp.SubqueryType))
            .TransformNameToCamelCase()
            .InitSchema()
            .SetEnumValues<ProductOptionSubtypeMpQueryType>();

        return operation;
    }
}
