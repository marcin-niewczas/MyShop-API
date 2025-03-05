using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands.ECommerce.Orders;
using MyShop.Application.Dtos.ECommerce.Orders;
using MyShop.Application.Dtos.ValidatorParameters.ECommerce;
using MyShop.Application.Queries.ECommerce.Orders;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Responses;
using MyShop.Application.Utils;
using MyShop.Core.Dtos.Account;
using MyShop.Core.Dtos.ECommerce;
using MyShop.Infrastructure.Swagger.Operations.ECommerce;

namespace MyShop.API.ApiEndpoints.ECommerce.EndpointsGroups;

public static class OrderEndpointsGroup
{
    public static RouteGroupBuilder MapEcommerceOrderEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/orders")
           .WithTags("Orders")
           .MapEndpoints();

        return app;
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("/", GetPagedDataAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .WithOpenApi(GetPagedOrdersEcOpenApi.ModifyOperation)
            .RequireAuthorization(PolicyNames.HasCustomerPermission);

        app.MapPost("/", CreateAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization(PolicyNames.HasGuestPermission);

        app.MapGet("/{id:guid}", GetFullOrderDataAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.HasGuestPermission);

        app.MapDelete("/{id:guid}/cancellation", CancelOrderAsync)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.HasCustomerPermission);

        app.MapGet("/{id:guid}/status", GetOrderStatusAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.HasGuestPermission);

        app.MapGet("/{id:guid}/invoices/{invoiceId:guid}", GetInvoiceAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.HasCustomerPermission);

        app.MapGet("/validator-parameters", GetOrderValidatorParametersAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .AllowAnonymous()
            .RequireAuthorization(PolicyNames.HasGuestPermission);

        return app;
    }

    private static async Task<Ok<ApiPagedResponse<OrderAcDto>>> GetPagedDataAsync(
        [AsParameters] GetPagedOrdersEc query,
        [FromServices] IQueryHandler<GetPagedOrdersEc, ApiPagedResponse<OrderAcDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Created<ApiIdResponse>> CreateAsync(
        [FromBody] CreateOrderEc command,
        [FromServices] ICommandHandler<CreateOrderEc, ApiIdResponse> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Created((string?)null, await handler.HandleAsync(command, cancellationToken));

    private static async Task<Ok<ApiResponse<OrderWithProductsEcDto>>> GetFullOrderDataAsync(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetOrderEc, ApiResponse<OrderWithProductsEcDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(new(id), cancellationToken));

    private static async Task<Ok<ApiResponse<OrderStatusEcDto>>> GetOrderStatusAsync(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetOrderStatusEc, ApiResponse<OrderStatusEcDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(new(id), cancellationToken));

    private static async Task<FileStreamHttpResult> GetInvoiceAsync(
        [AsParameters] GetInvoiceEc query,
        [FromServices] IQueryHandler<GetInvoiceEc, MemoryStream> handler,
        CancellationToken cancellationToken
        )
    {
        var result = await handler.HandleAsync(query, cancellationToken);
       
        return TypedResults.File(result, "application/pdf", "invoice");
    }

    private static async Task<NoContent> CancelOrderAsync(
        [AsParameters] CancelOrderEc command,
        [FromServices] ICommandHandler<CancelOrderEc> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static Task<Ok<ApiResponse<OrderValidatorParametersEcDto>>> GetOrderValidatorParametersAsync()
        => Task.FromResult(TypedResults.Ok(new ApiResponse<OrderValidatorParametersEcDto>(new())));
}
