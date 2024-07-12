using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.CommandHandlers;
using MyShop.Application.Commands.ManagementPanel.MainPageSections;
using MyShop.Application.Dtos.ManagementPanel.MainPageSections;
using MyShop.Application.Dtos.ValidatorParameters.ManagementPanel;
using MyShop.Application.Dtos;
using MyShop.Application.Queries.ManagementPanel.MainPageSections;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Responses;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Infrastructure.Swagger.Operations.ManagementPanel;

namespace MyShop.API.ApiEndpoints.ManagementPanel.EndpointsGroups;

public static class MainPageSectionEndpointsGroup
{
    public static RouteGroupBuilder MapManagementPanelMainPageSectionEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/main-page-sections")
           .WithTags("MainPageSections")
           .MapEndpoints();

        return app;
    }

    private static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", GetAllMainPageSectionsAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        app.MapPatch("/", UpdatePositionsOfMainPageSectionsAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("/{id:guid}", GetMainPageSectionAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapDelete("/{id:guid}", RemoveMainPageSectionsAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("/count", GetMainPageSectionsCountAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        app.MapPost("/website-hero-sections", CreateWebsiteHeroSectionAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        app.MapPatch("/website-hero-sections/{id:guid}", UpdateWebsiteHeroSectionAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        app.MapGet("/website-hero-sections/{id:guid}/website-hero-section-items", GetPagedWebsiteHeroSectionItemsAsync)
           .AddEndpointFilter<ModelValidateEndpointFilter>()
           .ProducesValidationProblem()
           .ProducesProblem(StatusCodes.Status401Unauthorized)
           .ProducesProblem(StatusCodes.Status403Forbidden)
           .WithOpenApi(GetPagedWebsiteHeroSectionItemsMpOpenApi.ModifyOperation);

        app.MapPost("/website-hero-sections/{id:guid}/website-hero-section-items", CreateWebsiteHeroSectionItemAsync)
            .DisableAntiforgery()
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPatch("/website-hero-sections/{id:guid}/website-hero-section-items/positions", UpdatePositionsOfWebsiteHeroSectionItemsAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("/website-hero-sections/{id:guid}/website-hero-section-items/validator-parameters", GetWebsiteHeroSectionItemValidatorParametersAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("/website-hero-section-items/{id:guid}", GetWebsiteHeroSectionItemAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPut("/website-hero-section-items/{id:guid}", UpdateWebsiteHeroSectionItemAsync)
            .DisableAntiforgery()
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapDelete("/website-hero-section-items/{id:guid}", RemoveWebsiteHeroSectionItemAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPatch("/website-hero-section-items/{id:guid}/activity-statuses", ChangeActivityStatusOfWebsiteHeroSectionItemAsync)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapPost("/website-product-carousel-sections", CreateWebsiteProductCarouselSectionAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        app.MapGet("/validator-parameters", GetMainPageSectionValidatorParametersAsync)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        return app;
    }

    private static async Task<Ok<ApiResponseWithCollection<MainPageSectionMpDto>>> GetAllMainPageSectionsAsync(
        [FromServices] IQueryHandler<GetAllMainPageSectionsMp, ApiResponseWithCollection<MainPageSectionMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(new(), cancellationToken));

    private static async Task<NoContent> UpdatePositionsOfMainPageSectionsAsync(
       [FromBody] UpdatePositionsOfMainPageSectionsMp command,
       [FromServices] ICommandHandler<UpdatePositionsOfMainPageSectionsMp> handler,
       CancellationToken cancellationToken
       )
    {
        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<Ok<ApiResponse<MainPageSectionMpDto>>> GetMainPageSectionAsync(
        [AsParameters] GetMainPageSectionMp query,
        [FromServices] IQueryHandler<GetMainPageSectionMp, ApiResponse<MainPageSectionMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<NoContent> RemoveMainPageSectionsAsync(
       [AsParameters] RemoveMainPageSectionMp command,
       [FromServices] ICommandHandler<RemoveMainPageSectionMp> handler,
       CancellationToken cancellationToken
       )
    {
        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<Ok<ApiResponse<ValueDto<int>>>> GetMainPageSectionsCountAsync(
        [AsParameters] GetMainPageSectionsCountMp query,
        [FromServices] IQueryHandler<GetMainPageSectionsCountMp, ApiResponse<ValueDto<int>>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiIdResponse>> CreateWebsiteHeroSectionAsync(
        [FromBody] CreateWebsiteHeroSectionMp command,
        [FromServices] ICommandHandler<CreateWebsiteHeroSectionMp, ApiIdResponse> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(command, cancellationToken));

    private static async Task<NoContent> UpdateWebsiteHeroSectionAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateWebsiteHeroSectionMp command,
        [FromServices] ICommandHandler<UpdateWebsiteHeroSectionMp> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.Id)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.Id)} in body.");

        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<Ok<ApiPagedResponse<WebsiteHeroSectionItemMpDto>>> GetPagedWebsiteHeroSectionItemsAsync(
        [AsParameters] GetPagedWebsiteHeroSectionItemsMp query,
        [FromServices] IQueryHandler<GetPagedWebsiteHeroSectionItemsMp, ApiPagedResponse<WebsiteHeroSectionItemMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiIdResponse>> CreateWebsiteHeroSectionItemAsync(
        [FromRoute] Guid id,
        [FromForm] CreateWebsiteHeroSectionItemMp command,
        [FromServices] ICommandHandler<CreateWebsiteHeroSectionItemMp, ApiIdResponse> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.WebsiteHeroSectionId)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.WebsiteHeroSectionId)} in form.");

        return TypedResults.Ok(await handler.HandleAsync(command, cancellationToken));
    }

    private static async Task<NoContent> UpdatePositionsOfWebsiteHeroSectionItemsAsync(
        [FromRoute] Guid id,
        [FromBody] UpdatePositionsOfActiveWebsiteHeroSectionItemsMp command,
        [FromServices] ICommandHandler<UpdatePositionsOfActiveWebsiteHeroSectionItemsMp> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.WebsiteHeroSectionId)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.WebsiteHeroSectionId)} in body.");

        await handler.HandleAsync(command, cancellationToken);

        return TypedResults.NoContent();
    }

    private static async Task<Ok<ApiResponse<WebsiteHeroSectionItemValidatorParametersMpDto>>> GetWebsiteHeroSectionItemValidatorParametersAsync(
        [AsParameters] GetWebsiteHeroSectionItemValidatorParametersMp query,
        [FromServices] IQueryHandler<GetWebsiteHeroSectionItemValidatorParametersMp, ApiResponse<WebsiteHeroSectionItemValidatorParametersMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiResponse<WebsiteHeroSectionItemMpDto>>> GetWebsiteHeroSectionItemAsync(
        [AsParameters] GetWebsiteHeroSectionItemMp query,
        [FromServices] IQueryHandler<GetWebsiteHeroSectionItemMp, ApiResponse<WebsiteHeroSectionItemMpDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<NoContent> UpdateWebsiteHeroSectionItemAsync(
        [FromRoute] Guid id,
        [FromForm] UpdateWebsiteHeroSectionItemMp command,
        [FromServices] ICommandHandler<UpdateWebsiteHeroSectionItemMp> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.Id)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.Id)} in form.");

        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> RemoveWebsiteHeroSectionItemAsync(
        [AsParameters] RemoveWebsiteHeroSectionItemMp command,
        [FromServices] ICommandHandler<RemoveWebsiteHeroSectionItemMp> handler,
        CancellationToken cancellationToken
        )
    {
        await handler.HandleAsync(command, cancellationToken);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> ChangeActivityStatusOfWebsiteHeroSectionItemAsync(
        [FromRoute] Guid id,
        [FromBody] ChangeActivityStatusOfWebsiteHeroSectionItemMp command,
        [FromServices] ICommandHandler<ChangeActivityStatusOfWebsiteHeroSectionItemMp> handler,
        CancellationToken cancellationToken
        )
    {
        if (id != command.Id)
            throw new BadRequestException($"{nameof(IEntity.Id)} in route must be equals {nameof(command.Id)} in body.");

        await handler.HandleAsync(command, cancellationToken);

        return TypedResults.NoContent();
    }

    private static async Task<Ok<ApiIdResponse>> CreateWebsiteProductCarouselSectionAsync(
        [FromBody] CreateWebsiteProductCarouselSectionMp command,
        [FromServices] ICommandHandler<CreateWebsiteProductCarouselSectionMp, ApiIdResponse> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(command, cancellationToken));

    private static Task<Ok<ApiResponse<MainPageSectionValidatorParametersMpDto>>> GetMainPageSectionValidatorParametersAsync()
        => Task.FromResult(TypedResults.Ok(new ApiResponse<MainPageSectionValidatorParametersMpDto>(new())));
}
