using Microsoft.Extensions.Logging;
using MyShop.Application.Abstractions;
using MyShop.Infrastructure.Commands;
using Quartz;

namespace MyShop.Infrastructure.CronJobs;
internal sealed class OrderPaymentStatusJob(
    ILogger<OrderPaymentStatusJob> logger,
    IMessageBroker messageBroker
    ) : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Trigger {JobName} job.", context.JobDetail.Key.Name);

        return messageBroker.PublishAsync(new ChangeOrdersPaymentStatus());
    }
}
