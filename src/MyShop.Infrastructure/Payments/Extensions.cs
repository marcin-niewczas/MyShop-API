using Microsoft.Extensions.DependencyInjection;
using MyShop.Infrastructure.Payments.Clients.MyShopPay;
using MyShop.Infrastructure.Payments.Services;
using MyShop.Infrastructure.Payments.Startegies;
using System.Reflection;

namespace MyShop.Infrastructure.Payments;
internal static class Extensions
{
    public static IServiceCollection AddPayments(this IServiceCollection services)
    {
        services.AddSingleton<IMyShopPayHttpClient, MyShopPayHttpClient>();
        services.AddScoped<IPaymentService, PaymentService>();

        services.Scan(s => s.FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(c => c.AssignableTo(typeof(IPaymentStrategy))).AsImplementedInterfaces().WithSingletonLifetime()
            );

        return services;
    }
}
