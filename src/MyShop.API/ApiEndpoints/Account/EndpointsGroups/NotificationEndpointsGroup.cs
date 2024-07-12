using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands.Account.Notifications;
using MyShop.Application.Dtos;
using MyShop.Application.Queries.Account.Notifications;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Responses;
using MyShop.Application.Responses.ExtensionResponses;
using MyShop.Infrastructure.Swagger.Operations.Account;

namespace MyShop.API.ApiEndpoints.Account.EndpointsGroups;

public static class NotificationEndpointsGroup
{
    public static RouteGroupBuilder MapAccountNotficicationEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("notifications")
            .WithTags("Notifications")
            .MapEndpoints();

        return app;
    }

    private static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", GetPagedDataAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithOpenApi(GetPagedNotificationsAcOpenApi.ModifyOperation);

        app.MapPatch("/", SetAllNotificationAsReadAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        app.MapPatch("/{id:guid}", SetNotificationAsReadAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        return app;
    }

    private static async Task<Ok<GetNotificationsApiPagedResponse>> GetPagedDataAsync(
        [AsParameters] GetPagedNotificationsAc query,
        IQueryHandler<GetPagedNotificationsAc, GetNotificationsApiPagedResponse> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiResponse<ValueDto<int>>>> SetAllNotificationAsReadAsync(
       [FromServices] ICommandHandler<SetAllNotificationsAsRead, ApiResponse<ValueDto<int>>> handler,
       CancellationToken cancellationToken
       ) => TypedResults.Ok(await handler.HandleAsync(new(), cancellationToken));


    private static async Task<Ok<ApiResponse<ValueDto<int>>>> SetNotificationAsReadAsync(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<SetNotificationAsRead, ApiResponse<ValueDto<int>>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(new(id), cancellationToken));
}
