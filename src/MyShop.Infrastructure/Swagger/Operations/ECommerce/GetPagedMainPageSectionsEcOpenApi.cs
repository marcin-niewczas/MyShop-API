using Microsoft.OpenApi.Models;
using MyShop.Application.Queries.ECommerce.MainPageSections;

namespace MyShop.Infrastructure.Swagger.Operations.ECommerce;
public sealed class GetPagedMainPageSectionsEcOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation
            .SetPaginationParameters()
            .SetPageSizeParameter(nameof(GetPagedMainPageSectionsEc.ProductCarouselItemsCount), true);

        return operation;
    }
}
