using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands.Account.Users;
using MyShop.Application.Dtos;
using MyShop.Application.Dtos.Account.Users;
using MyShop.Application.Dtos.ValidatorParameters.Account;
using MyShop.Application.Queries.Account.Users;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Responses;
using MyShop.Core.Dtos.Auth;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Infrastructure.Swagger.Operations.Account;

namespace MyShop.API.ApiEndpoints.Account.EndpointsGroups;

public static class UserEndpointsGroup
{
    public static RouteGroupBuilder MapAccountUserEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/users")
           .WithTags("Users")
           .MapEndpoints();

        return app;
    }

    private static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPut("/", UpdateUserAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        app.MapGet("/active-devices", GetUserActiveDevicesAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .WithOpenApi(GetPagedUserActiveDevicesAcOpenApi.ModifyOperation);

        app.MapGet("/addresses", GetUserAddressesAsync)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        app.MapGet("/addresses/count", GetUserAddressesCountAsync)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        app.MapPost("/addresses", CreateUserAddressAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        app.MapGet("/addresses/{id:guid}", GetUserAddressAsync)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPut("/addresses/{id:guid}", UpdateUserAddressAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapDelete("/addresses/{id:guid}", RemoveUserAddressAsync)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPatch("/e-mails", UpdateUserEmailAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        app.MapPatch("/passwords", UpdateUserPasswordAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        app.MapPatch("/photos", UploadUserPhotoAsync)
            .DisableAntiforgery()
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapDelete("/photos", RemoveUserPhotoAsync)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("/validator-parameters", GetUserValidatorParametersAsync)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        app.MapGet("/addresses/validator-parameters", GetUserAddressValidatorParametersAsync)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        app.MapGet("/photos/validator-parameters", GetUserPhotoValidatorParametersAsync)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        app.MapGet("/securites/validator-parameters", GetSecurityValidatorParametersAsync)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        return app;
    }

    private static async Task<Ok<ApiResponse<UserDto>>> UpdateUserAsync(
        [FromBody] UpdateRegisteredUserAc command,
        [FromServices] ICommandHandler<UpdateRegisteredUserAc, ApiResponse<UserDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(command, cancellationToken));

    private static async Task<Ok<ApiPagedResponse<UserActiveDeviceAcDto>>> GetUserActiveDevicesAsync(
        [AsParameters] GetPagedUserActiveDevicesAc query,
        [FromServices] IQueryHandler<GetPagedUserActiveDevicesAc, ApiPagedResponse<UserActiveDeviceAcDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiResponseWithCollection<UserAddressAcDto>>> GetUserAddressesAsync(
        [FromServices] IQueryHandler<GetUserAddressesAc, ApiResponseWithCollection<UserAddressAcDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(new(), cancellationToken));

    private static async Task<Ok<ApiResponse<ValueDto<int>>>> GetUserAddressesCountAsync(
       [FromServices] IQueryHandler<GetUserAddressesCountAc, ApiResponse<ValueDto<int>>> handler,
       CancellationToken cancellationToken
       ) => TypedResults.Ok(await handler.HandleAsync(new(), cancellationToken));

    private static async Task<Created<ApiResponse<UserAddressAcDto>>> CreateUserAddressAsync(
        [FromBody] CreateRegisteredUserAddressAc command,
        [FromServices] ICommandHandler<CreateRegisteredUserAddressAc, ApiResponse<UserAddressAcDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Created((string?)null, await handler.HandleAsync(command, cancellationToken));

    private static async Task<Ok<ApiResponse<UserAddressAcDto>>> GetUserAddressAsync(
       [FromRoute] Guid id,
       [FromServices] IQueryHandler<GetUserAddressAc, ApiResponse<UserAddressAcDto>> handler,
       CancellationToken cancellationToken
       ) => TypedResults.Ok(await handler.HandleAsync(new(id), cancellationToken));

    private static async Task<Ok<ApiResponse<UserAddressAcDto>>> UpdateUserAddressAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateRegisteredUserAddressAc command,
        [FromServices] ICommandHandler<UpdateRegisteredUserAddressAc, ApiResponse<UserAddressAcDto>> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.Id)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.Id)} in body.");

        return TypedResults.Ok(await handler.HandleAsync(command, cancellationToken));
    }

    private static async Task<NoContent> RemoveUserAddressAsync(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<RemoveRegisteredUserAddressAc> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(new(id), cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> UpdateUserEmailAsync(
        [FromBody] UpdateRegisteredUserEmailAc command,
        [FromServices] ICommandHandler<UpdateRegisteredUserEmailAc> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> UpdateUserPasswordAsync(
        [FromBody] UpdateRegisteredUserPasswordAc command,
        [FromServices] ICommandHandler<UpdateRegisteredUserPasswordAc> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<Ok<ApiResponse<UserDto>>> UploadUserPhotoAsync(
        [AsParameters] UploadUserPhotoAc command,
        [FromServices] ICommandHandler<UploadUserPhotoAc, ApiResponse<UserDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(command, cancellationToken));

    private static async Task<Ok<ApiResponse<UserDto>>> RemoveUserPhotoAsync(
        [AsParameters] RemoveUserPhotoAc command,
        [FromServices] ICommandHandler<RemoveUserPhotoAc, ApiResponse<UserDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(command, cancellationToken));

    private static Task<Ok<ApiResponse<UserValidatorParametersAcDto>>> GetUserValidatorParametersAsync()
        => Task.FromResult(TypedResults.Ok(new ApiResponse<UserValidatorParametersAcDto>(new())));

    private static Task<Ok<ApiResponse<UserAddressValidatorParametersAcDto>>> GetUserAddressValidatorParametersAsync()
        => Task.FromResult(TypedResults.Ok(new ApiResponse<UserAddressValidatorParametersAcDto>(new())));

    private static Task<Ok<ApiResponse<SecurityValidatorParametersAcDto>>> GetSecurityValidatorParametersAsync()
        => Task.FromResult(TypedResults.Ok(new ApiResponse<SecurityValidatorParametersAcDto>(new())));

    private static Task<Ok<ApiResponse<UserPhotoValidatorParametersAcDto>>> GetUserPhotoValidatorParametersAsync()
        => Task.FromResult(TypedResults.Ok(new ApiResponse<UserPhotoValidatorParametersAcDto>(new())));
}
