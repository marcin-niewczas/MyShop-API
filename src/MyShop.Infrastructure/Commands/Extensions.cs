using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.CommandHandlers;
using System.Reflection;

namespace MyShop.Infrastructure.Commands;
internal static class Extension
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services.Scan(s => s.FromAssemblies(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(Application.Extensions))!)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>))).AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>))).AsImplementedInterfaces().WithScopedLifetime());

        return services;
    }
}
