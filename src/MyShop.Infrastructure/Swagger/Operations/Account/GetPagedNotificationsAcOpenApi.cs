using Microsoft.OpenApi.Models;
using MyShop.Application.Queries.Account.Notifications;
using MyShop.Core.RepositoryQueryParams.Account;

namespace MyShop.Infrastructure.Swagger.Operations.Account;
public sealed class GetPagedNotificationsAcOpenApi : IModifyOpenApiOperation
{
    public static OpenApiOperation ModifyOperation(OpenApiOperation operation)
    {
        operation.SetPaginationParameters();
        operation.SetSortParameters<GetPagedNotificationsAcSortBy>();
        operation.SetTimestampParameters();
        operation.ForParameter(nameof(GetPagedNotificationsAc.WithUnreadNotificationCount))
                 .TransformNameToCamelCase()
                 .InitSchema()
                 .SetType("boolean");

        return operation;
    }
}
