using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Infrastructure.DataAccessLayer.MainDatabase.Initializer;
using MyShop.Infrastructure.DataAccessLayer.MainDatabase.Interceptors;
using MyShop.Infrastructure.Options;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase;
internal static class Extensions
{
    public static IServiceCollection AddMainDatabase(
        this IServiceCollection services,
        IWebHostEnvironment environment
        )
    {
        services.AddDbContext<MainDbContext>((provider, options) =>
        {
            options.UseSqlServer(provider.GetRequiredService<IOptions<MainDbOptions>>().Value.ConnectionString);
            options.AddInterceptors(provider.GetRequiredService<MainDbSaveChangesInterceptor>());
        });

        services.AddInterceptors();

        if (environment.IsDevelopment())
        {
            services.AddHostedService<MainDevDbInitializerHostedService>();
        }

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
