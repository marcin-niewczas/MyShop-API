using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace MyShop.Infrastructure.CronJobs;
internal static class Extensions
{
    public static IServiceCollection AddCronJobs(
        this IServiceCollection services, 
        IConfiguration configuration
        )
    {
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey(nameof(OrderPaymentStatusJob));

            q.AddJob<OrderPaymentStatusJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity($"{nameof(OrderPaymentStatusJob)}-trigger")
                .WithCronSchedule("0 * * ? * *")
            );
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }
}
