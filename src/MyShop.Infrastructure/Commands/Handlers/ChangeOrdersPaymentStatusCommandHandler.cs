using MassTransit;
using Microsoft.Extensions.Logging;
using MyShop.Application.CommandHandlers;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Notifications;
using MyShop.Core.Models.Orders;
using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.Notifications;
using MyShop.Core.ValueObjects.Orders;
using MyShop.Infrastructure.CronJobs;
using MyShop.Infrastructure.Notifications.Orders;
using MyShop.Infrastructure.Notifications.Senders.Interfaces;
using MyShop.Infrastructure.Payments.Services;

namespace MyShop.Infrastructure.Commands.Handlers;
internal sealed class ChangeOrdersPaymentStatusCommandHandler(
    IUnitOfWork unitOfWork,
    ILogger<ChangeOrdersPaymentStatusCommandHandler> logger,
    IPaymentService paymentService,
    IOrderNotificationsSender orderNotificationsSender
    ) : ICommandHandler<ChangeOrdersPaymentStatus>, 
        IConsumer<ChangeOrdersPaymentStatus>
{   
    public async Task HandleAsync(ChangeOrdersPaymentStatus command, CancellationToken cancellationToken = default)
    {
        var orders = await unitOfWork.OrderRepository.GetByPredicateAsync(
            includeExpressions: [i => i.User, i => i.OrderStatusHistories],
            predicate: e => e.Status == OrderStatus.WaitingForPayment,
            withTracking: true,
            cancellationToken: cancellationToken
            );

        if (orders.Count <= 0)
        {
            logger.LogInformation(
            "[{CronJobName}] The {Status} of any {Order} hasn't been changed.",
            nameof(OrderPaymentStatusJob),
            nameof(Order.Status),
            nameof(Order)
            );

            return;
        }

        var tasks = orders
            .Select(o => paymentService.UpdatePaymentOrderStatusAsync(o, cancellationToken));

        var updatedOrders = (await Task.WhenAll(tasks))
            .Where(r => r.IsUpdated)
            .Select(r => r.Order)
            .ToList();

        if (updatedOrders.Count > 0)
        {
            List<Notification> notificationList = [];
            List<(Notification? Notification, Order Order)> notificationOrderList = [];
            Notification? notification = null;

            foreach (var order in updatedOrders)
            {
                if (order.User is RegisteredUser)
                {
                    notification = new(
                        NotificationType.Order,
                        $"The {nameof(Order)} status has been changed to {order.Status}.",
                        order.User.Id,
                        order.Id.ToString()
                        );

                    notificationList.Add(notification);
                }
                else
                {
                    notification = null;
                }

                notificationOrderList.Add((notification, order));
            }

            if (notificationList.Count > 0)
            {
                await unitOfWork.AddRangeAsync(
                    notificationList,
                    cancellationToken
                    );
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await Task.WhenAll(notificationOrderList.Select(no => orderNotificationsSender.SendAsync(
                    no.Order,
                    OrderNotificationType.UpdateStatus,
                    no.Notification,
                    cancellationToken: cancellationToken
                    )));
        }

        logger.LogInformation(
            "[{HandlerName}] The {Status} has been changed for {UpdatedOrdersCount} {Orders}.",
            nameof(ChangeOrdersPaymentStatusCommandHandler),
            nameof(Order.Status),
            updatedOrders.Count,
            nameof(User.Orders)
            );
    }

    public Task Consume(ConsumeContext<ChangeOrdersPaymentStatus> context)
        => HandleAsync(context.Message);
}
