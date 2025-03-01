using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.Utils;
using System.Reflection;
using static MyShop.Application.Utils.ExtensionsHelper;

namespace MyShop.Application.QueryHandlers;
internal static class Extensions
{
    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        services.ScanAndRegisterGenericDependencies(
            typeof(IQueryHandler<,>),
            DependencyLifecycle.Scoped,
            Assembly.GetExecutingAssembly()
            );

        return services;
    }
}
