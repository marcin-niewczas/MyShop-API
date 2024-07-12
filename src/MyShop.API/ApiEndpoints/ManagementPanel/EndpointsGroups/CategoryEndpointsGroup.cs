using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands.ManagementPanel.Categories;
using MyShop.Application.Dtos.ValidatorParameters.ManagementPanel;
using MyShop.Application.Queries.ManagementPanel.Categories;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Responses;
using MyShop.Core.Dtos.ManagementPanel;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Infrastructure.Swagger.Operations.ManagementPanel;

namespace MyShop.API.ApiEndpoints.ManagementPanel.EndpointsGroups;

public static class CategoryEndpointsGroup
{
    public static RouteGroupBuilder MapManagementPanelCategoryEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/categories")
           .WithTags("Categories")
           .MapEndpoints();

        return app;
    }

    private static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", GetPagedDataAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .WithOpenApi(GetPagedCategoriesMpOpenApi.ModifyOperation);

        app.MapPost("/", CreateAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        app.MapGet("/{id:guid}", GetByIdAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi(GetCategoryMpOpenApi.ModifyOperation);

        app.MapPut("/{id:guid}", UpdateAsync)
           .AddEndpointFilter<ModelValidateEndpointFilter>()
           .ProducesValidationProblem()
           .ProducesProblem(StatusCodes.Status401Unauthorized)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapDelete("/{id:guid}", RemoveAsync)
           .ProducesValidationProblem()
           .ProducesProblem(StatusCodes.Status401Unauthorized)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("/{rootId:guid}/product-categories", GetPagedProductCategoriesByCategoryRootIdAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi(GetPagedProductCategoriesByCategoryRootIdMpOpenApi.ModifyOperation);

        app.MapGet("/validator-parameters", GetCategoryValidatorParametersAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        return app;
    }

    private static async Task<Ok<ApiPagedResponse<CategoryMpDto>>> GetPagedDataAsync(
        [AsParameters] GetPagedCategoriesMp query,
        [FromServices] IQueryHandler<GetPagedCategoriesMp, ApiPagedResponse<CategoryMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Created<ApiIdResponse>> CreateAsync(
        [FromBody] CreateCategoryMp command,
        [FromServices] ICommandHandler<CreateCategoryMp, ApiIdResponse> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Created((string?)null, await handler.HandleAsync(command, cancellationToken));

    private static async Task<Ok<ApiResponse<CategoryMpDto>>> GetByIdAsync(
        [AsParameters] GetCategoryMp query,
        [FromServices] IQueryHandler<GetCategoryMp, ApiResponse<CategoryMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiResponse<CategoryMpDto>>> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateCategoryMp command,
        [FromServices] ICommandHandler<UpdateCategoryMp, ApiResponse<CategoryMpDto>> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.Id)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.Id)} in body.");

        return TypedResults.Ok(await handler.HandleAsync(command, cancellationToken));
    }

    private static async Task<NoContent> RemoveAsync(
        [AsParameters] RemoveCategoryMp command,
        [FromServices] ICommandHandler<RemoveCategoryMp> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<Ok<ApiPagedResponse<CategoryMpDto>>> GetPagedProductCategoriesByCategoryRootIdAsync(
        [AsParameters] GetPagedProductCategoriesByCategoryRootIdMp query,
        [FromServices] IQueryHandler<GetPagedProductCategoriesByCategoryRootIdMp, ApiPagedResponse<CategoryMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static Task<Ok<ApiResponse<CategoryValidatorParametersMpDto>>> GetCategoryValidatorParametersAsync()
        => Task.FromResult(TypedResults.Ok(new ApiResponse<CategoryValidatorParametersMpDto>(new())));
}
