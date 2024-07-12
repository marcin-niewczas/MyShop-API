using Microsoft.OpenApi.Models;
using MyShop.Application.Queries.ManagementPanel.MainPageSections;

namespace MyShop.Infrastructure.Swagger.Operations.ManagementPanel;
public sealed class GetPagedWebsiteHeroSectionItemsMpOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation.SetPaginationParameters();
        operation
            .ForParameter(nameof(GetPagedWebsiteHeroSectionItemsMp.Active))
            .TransformNameToCamelCase()
            .InitSchema()
            .SetType("boolean");

        return operation;
    }
}
