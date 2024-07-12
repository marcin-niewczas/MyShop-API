using Microsoft.Extensions.DependencyInjection;

namespace MyShop.Application.QueryHandlers;
internal static class Extensions
{
    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        services.Scan(s => s.FromCallingAssembly()
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
