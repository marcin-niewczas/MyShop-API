using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyShop.API.ApiEndpoints.EndpointsFilters;
using MyShop.Application.Dtos.ECommerce.Categories;
using MyShop.Application.Dtos.ECommerce.Products;
using MyShop.Application.Queries.ECommerce.Categories;
using MyShop.Application.QueryHandlers;
using MyShop.Application.Responses;
using MyShop.Infrastructure.Swagger.Operations.ECommerce;

namespace MyShop.API.ApiEndpoints.ECommerce.EndpointsGroups;

public static class CategoryEndpointsGroup
{
    public static RouteGroupBuilder MapEcommerceCategoryEndpointsGroup(this RouteGroupBuilder app)
    {
        app.MapGroup("/categories")
           .WithTags("Categories")
           .MapCategoryEndpoints();

        return app;
    }

    private static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", GetPagedCategoriesAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .WithOpenApi(GetPagedCategoriesEcOpenApi.ModifyOperation);

        app.MapGet("/{encodedName}", GetCategoryByEncodedHierarchyNameAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("/{encodedCategoryName}/product-filters", GetProductFiltersAsync)
            .AddEndpointFilter<ModelValidateEndpointFilter>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApi(GetProductFiltersByCategoryIdEcOpenApi.ModifyOperation);

        return app;
    }

    private static async Task<Ok<ApiPagedResponse<CategoryEcDto>>> GetPagedCategoriesAsync(
        [AsParameters] GetPagedCategoriesEc query,
        [FromServices] IQueryHandler<GetPagedCategoriesEc, ApiPagedResponse<CategoryEcDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiResponse<CategoryEcDto>>> GetCategoryByEncodedHierarchyNameAsync(
        [AsParameters] GetCategoryEc query,
        [FromServices] IQueryHandler<GetCategoryEc, ApiResponse<CategoryEcDto>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));

    private static async Task<Ok<ApiResponse<ProductFiltersDtoEc>>> GetProductFiltersAsync(
        [AsParameters] GetProductFiltersByCategoryIdEc query,
        [FromServices] IQueryHandler<GetProductFiltersByCategoryIdEc, ApiResponse<ProductFiltersDtoEc>> handler,
        CancellationToken cancellationToken
        ) => TypedResults.Ok(await handler.HandleAsync(query, cancellationToken));
}
