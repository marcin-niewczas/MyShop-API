using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MyShop.Application.Utils;
public static class ExtensionsHelper
{
    public enum DependencyLifecycle
    {
        Singleton = 0,
        Scoped,
        Transient
    }

    public static IServiceCollection ScanAndRegisterGenericDependencies(
        this IServiceCollection services,
        Type serviceType,
        DependencyLifecycle dependencyLifecycle,
        Assembly assembly
        )
    {
        var types = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == serviceType))
            .ToList();

        foreach (var type in types)
        {
            var interfaceType = type.GetInterfaces().First(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == serviceType);

            switch (dependencyLifecycle)
            {
                case DependencyLifecycle.Singleton:
                    services.AddSingleton(interfaceType, type);
                    break;
                case DependencyLifecycle.Scoped:
                    services.AddScoped(interfaceType, type);
                    break;
                case DependencyLifecycle.Transient:
                    services.AddTransient(interfaceType, type);
                    break;
                default:
                    throw new ArgumentException($"Wrong {nameof(DependencyLifecycle)}.");
            }
        }

        return services;
    }

    public static IServiceCollection ScanAndRegisterGenericDependencies(
        this IServiceCollection services,
        Type serviceType,
        DependencyLifecycle dependencyLifecycle,
        Assembly[] assemblies
        )
    {
        foreach (var assembly in assemblies)
        {
            services.ScanAndRegisterGenericDependencies(serviceType, dependencyLifecycle, assembly);
        }

        return services;
    }

    public static IServiceCollection ScanAndRegisterDependenciesAsInterface(
        this IServiceCollection services,
        Type serviceType,
        DependencyLifecycle dependencyLifecycle,
        Assembly assembly
        )
    {
        var types = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i == serviceType))
            .ToList();

        foreach (var type in types)
        {
            var interfaceType = type.GetInterfaces().First(i => i == serviceType);

            switch (dependencyLifecycle)
            {
                case DependencyLifecycle.Singleton:
                    services.AddSingleton(interfaceType, type);
                    break;
                case DependencyLifecycle.Scoped:
                    services.AddScoped(interfaceType, type);
                    break;
                case DependencyLifecycle.Transient:
                    services.AddTransient(interfaceType, type);
                    break;
                default:
                    throw new ArgumentException($"Wrong {nameof(DependencyLifecycle)}.");
            }
        }

        return services;
    }
}