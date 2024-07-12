using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyShop.Core.Abstractions;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.Initializer;
internal sealed class MainDevDbInitializerHostedService(
    IServiceProvider serviceProvider
    ) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
        await dbContext.Database.MigrateAsync(cancellationToken);

        var passwordManager = scope.ServiceProvider.GetRequiredService<IPasswordManager>();

        var anyEmployees = await dbContext.Employees.AnyAsync(cancellationToken);

        if (!anyEmployees)
        {
            await dbContext.AddRangeAsync(
                MainDevDbInitData.GetEmployess(passwordManager),
                cancellationToken
                );
        }

        var anyCustomers = await dbContext.Customers.AnyAsync(cancellationToken);

        if (!anyCustomers)
        {
            await dbContext.AddRangeAsync(
                MainDevDbInitData.GetCustomers(passwordManager),
                cancellationToken
                );
        }

        var anyCategories = await dbContext.Categories.AnyAsync(cancellationToken);

        if (!anyCategories)
        {
            await dbContext.AddRangeAsync(
                MainDevDbInitData.GetCategories(),
                cancellationToken
                );
        }

        var anyProductDetailOptions = await dbContext.ProductDetailOptions.AnyAsync(cancellationToken);

        if (!anyProductDetailOptions)
        {
            await dbContext.AddRangeAsync(
                MainDevDbInitData.GetProductDetailOptions(),
                cancellationToken
                );
        }

        var anyProductVariantOptions = await dbContext.ProductVariantOptions.AnyAsync(cancellationToken);

        if (!anyProductVariantOptions)
        {
            await dbContext.AddRangeAsync(
                MainDevDbInitData.GetProductVariantOptions(),
                cancellationToken
                );
        }

        if (!anyCustomers || !anyCustomers || !anyCategories || !anyProductDetailOptions || !anyProductVariantOptions)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
