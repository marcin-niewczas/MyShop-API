using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Hosting;
using MyShop.Core.Exceptions;

namespace MyShop.Infrastructure.Exceptions;
internal sealed class GlobalExceptionHandler(
    IHostEnvironment hostEnvironment
    ) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        await ProducesProblemDetails(exception).ExecuteAsync(httpContext);

        return true;
    }

    private ProblemHttpResult ProducesProblemDetails(Exception exception)
    {
        if (hostEnvironment.IsDevelopment() || hostEnvironment.IsStaging())
        {
            return ProducesDevelopmentProblemDetails(exception);
        }

        return ProducesProductionProblemDetails(exception);
    }

    private static ProblemHttpResult ProducesDevelopmentProblemDetails(Exception exception)
    {
        var code = exception switch
        {
            CustomException customException => customException.StatusCode,
            OperationCanceledException => 499,
            _ => StatusCodes.Status500InternalServerError
        };

        return TypedResults.Problem(statusCode: code, detail: exception.Message, extensions: new Dictionary<string, object?>()
        {
            ["source"] = exception.Source,
            ["innerException"] = exception.InnerException,
            ["stackTrace"] = exception.StackTrace,
        });
    }

    private static ProblemHttpResult ProducesProductionProblemDetails(Exception exception)
    {
        var (code, message) = exception switch
        {
            CustomException customException => (customException.StatusCode, customException.Message),
            OperationCanceledException => (499, "Operation has been cancelled."),
            _ => (StatusCodes.Status500InternalServerError, null)
        };

        return TypedResults.Problem(statusCode: code, detail: message);
    }
}
