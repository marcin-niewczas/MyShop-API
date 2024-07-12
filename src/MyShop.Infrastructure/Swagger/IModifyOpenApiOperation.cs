using Microsoft.OpenApi.Models;

namespace MyShop.Infrastructure.Swagger;
internal interface IModifyOpenApiOperation
{
    static abstract OpenApiOperation ModifyOperation(OpenApiOperation operation);
}
