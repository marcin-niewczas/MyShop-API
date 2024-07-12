using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands.ManagementPanel.Orders;
using MyShop.Application.Dtos.ValidatorParameters.ManagementPanel;
using MyShop.Application.Queries.ManagementPanel.Orders;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Responses;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Infrastructure.Swagger.Operations.ManagementPanel;

namespace MyShop.API.ApiEndpoints.ManagementPanel.EndpointsGroups;

public static class OrderEndpointsGroup
{
    public static RouteGroupBuilder MapManagementPanelOrderEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/orders")
           .WithTags("Orders")
           .MapEndpoints();

        return app;
    }

    private static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", GetPagedOrdersAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .WithOpenApi(GetPagedOrdersMpOpenApi.ModifyOperation);

        app.MapGet("/{id:guid}", GetOrderAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPut("/{id:guid}", UpdateOrderAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        app.MapGet("/{id:guid}/details", GetOrderDetailsAsync)
           .ProducesProblem(StatusCodes.Status401Unauthorized)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("/validator-parameters", GetOrderValidatorParametersAsync)
           .ProducesProblem(StatusCodes.Status401Unauthorized);

        return app;
    }

    private static async Task<Ok<ApiPagedResponse<PagedOrderMpDto>>> GetPagedOrdersAsync(
        [AsParameters] GetPagedOrdersMp query,
        [FromServices] IQueryHandler<GetPagedOrdersMp, ApiPagedResponse<PagedOrderMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiResponse<OrderMpDto>>> GetOrderAsync(
        [AsParameters] GetOrderMp query,
        [FromServices] IQueryHandler<GetOrderMp, ApiResponse<OrderMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok> UpdateOrderAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateOrderMp command,
        [FromServices] ICommandHandler<UpdateOrderMp> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.Id)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.Id)} in body.");

        await handler.HandleAsync(command, cancellationToken);

        return TypedResults.Ok();
    }

    private static async Task<Ok<ApiResponse<OrderDetailsMpDto>>> GetOrderDetailsAsync(
        [AsParameters] GetOrderDetailsMp query,
        [FromServices] IQueryHandler<GetOrderDetailsMp, ApiResponse<OrderDetailsMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static Task<Ok<ApiResponse<OrderValidatorParametersMpDto>>> GetOrderValidatorParametersAsync()
        => Task.FromResult(TypedResults.Ok(new ApiResponse<OrderValidatorParametersMpDto>(new())));
}
