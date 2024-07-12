﻿using Microsoft.OpenApi.Models;

namespace MyShop.Infrastructure.Swagger.Operations.ManagementPanel;
public sealed class GetPagedProductVariantsByProductIdMpOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation.SetPaginationParameters();

        return operation;
    }
}
