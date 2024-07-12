using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.Validations.Interfaces;

namespace MyShop.Application.Validations;
internal static class Extensions
{
    public static IServiceCollection AddValidations(this IServiceCollection services)
    {
        services.AddSingleton<IValidationService, ValidationService>();

        return services;
    }
}
