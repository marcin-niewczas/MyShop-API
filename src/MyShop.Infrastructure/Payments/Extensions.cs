using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.Utils;
using MyShop.Infrastructure.Payments.Clients.MyShopPay;
using MyShop.Infrastructure.Payments.Services;
using MyShop.Infrastructure.Payments.Startegies;
using System.Reflection;
using static MyShop.Application.Utils.ExtensionsHelper;

namespace MyShop.Infrastructure.Payments;
internal static class Extensions
{
    public static IServiceCollection AddPayments(this IServiceCollection services)
    {
        services.AddSingleton<IMyShopPayHttpClient, MyShopPayHttpClient>();
        services.AddScoped<IPaymentService, PaymentService>();

        services.ScanAndRegisterDependenciesAsInterface(
            typeof(IPaymentStrategy),
            DependencyLifecycle.Singleton,
            Assembly.GetExecutingAssembly()
            );

        return services;
    }
}
