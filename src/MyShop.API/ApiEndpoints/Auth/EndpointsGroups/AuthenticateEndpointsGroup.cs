using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands.Auth;
using MyShop.Application.Dtos.Auth;
using MyShop.Application.Dtos.ValidatorParameters.Auth;
using MyShop.Application.Queries.Auth;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Responses;
using MyShop.Application.Utils;
using MyShop.Core.Dtos.Auth;

namespace MyShop.API.ApiEndpoints.Auth.EndpointsGroups;

public static class AuthenticateEndpointsGroup
{
    public static RouteGroupBuilder MapAuthenticateEndpointsGroup(this RouteGroupBuilder app)
    {
        app.WithTags("Authenticates")
           .MapAuthenticateEndpoints();

        return app;
    }

    private static IEndpointRouteBuilder MapAuthenticateEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/customers/sign-up", SignUpCustomerAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem();

        app.MapPost("/guests/sign-up", SignUpGuestAsync);

        app.MapPost("/refresh-access-token", RefreshAccessTokenAsync)
            .ProducesValidationProblem();

        app.MapPost("/sign-in", SignInAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem();

        app.MapGet("/users/me", GetUserMeAsync)
            .RequireAuthorization()
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        app.MapPost("/users/me/logout", LogoutUserAsync)
            .RequireAuthorization(PolicyNames.HasCustomerPermission)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        app.MapGet("/validator-parameters", GetAuthValidatorParametersAsync);

        return app;
    }

    private static async Task<Ok> SignUpCustomerAsync(
        [FromBody] SignUpCutomerAuth command,
        [FromServices] ICommandHandler<SignUpCutomerAuth> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.Ok();
    }

    private static async Task<Ok<ApiResponse<AuthDto>>> SignUpGuestAsync(
        [FromServices] ICommandHandler<SignUpGuestAuth, ApiResponse<AuthDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(new(), cancellationToken));

    private static async Task<Ok<ApiResponse<AuthDto>>> RefreshAccessTokenAsync(
        [FromBody] RefreshAccessTokenAuth refreshToken,
        [FromServices] ICommandHandler<RefreshAccessTokenAuth, ApiResponse<AuthDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(refreshToken, cancellationToken));

    private static async Task<Ok<ApiResponse<AuthDto>>> SignInAsync(
        [FromBody] SignInAuth command,
        [FromServices] ICommandHandler<SignInAuth, ApiResponse<AuthDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(command, cancellationToken));

    private static async Task<Ok<ApiResponse<UserDto>>> GetUserMeAsync(
        [FromServices] IQueryHandler<GetUserMeAuth, ApiResponse<UserDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(new GetUserMeAuth(), cancellationToken));

    private static async Task<Ok> LogoutUserAsync(
        [FromServices] ICommandHandler<LogoutAuth> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(new(), cancellationToken);
        return TypedResults.Ok();
    }

    private static Task<Ok<ApiResponse<AuthValidatorParametersDto>>> GetAuthValidatorParametersAsync()
        => Task.FromResult(TypedResults.Ok(new ApiResponse<AuthValidatorParametersDto>(new())));
}
