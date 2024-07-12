using MyShop.Application.Validations.Interfaces;
using MyShop.Core.Exceptions;
using MyShop.Core.Utils;

namespace MyShop.API.ApiEndpoints.EndpointsFilters;

internal sealed class ModelValidateEndpointFilter(IValidationService validationService) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var inputModels = context.Arguments
            .Where(e => e is IValidatable)
            .Cast<IValidatable>()
            .ToArray();

        if (inputModels.IsNullOrEmpty())
        {
            throw new ServerException($"The {nameof(ModelValidateEndpointFilter)} must validate at least one model.");
        }

        var validationSummary = validationService.ValidateModels(inputModels);

        if (!validationSummary.IsValid)
        {
            return TypedResults.ValidationProblem(
                errors: validationSummary.GetValidationProblemDictionary(),
                extensions: new Dictionary<string, object?>() { ["errorResultType"] = nameof(TypedResults.ValidationProblem) }
                );
        }

        return await next(context);
    }
}
