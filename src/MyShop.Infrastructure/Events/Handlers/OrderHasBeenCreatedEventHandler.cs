using MassTransit;
using MyShop.Application.Abstractions;
using MyShop.Application.Events;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Notifications;
using MyShop.Core.Models.Orders;
using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.Notifications;
using MyShop.Core.ValueObjects.Orders;
using MyShop.Infrastructure.Notifications.Orders;
using MyShop.Infrastructure.Notifications.Senders.Interfaces;
using MyShop.Infrastructure.Payments.Services;

namespace MyShop.Infrastructure.Events.Handlers;
internal sealed class OrderHasBeenCreatedEventHandler(
    IUnitOfWork unitOfWork,
    IOrderNotificationsSender orderNotificationsSender,
    IPaymentService paymentService
    ) : IEventHandler<OrderHasBeenCreated>, 
        IConsumer<OrderHasBeenCreated>
{
    public async Task HandleAsync(OrderHasBeenCreated @event, CancellationToken cancellationToken = default)
    {
        var order = await unitOfWork.OrderRepository.GetByIdAsync(
            id: @event.Id,
            includeExpressions: [i => i.User, i => i.OrderStatusHistories, i => i.OrderProducts],
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(Order), @event.Id);

        if (order.PaymentMethod != PaymentMethod.CashOnDelivery)
        {

            var paymentResponse = await paymentService.CreatePaymentAsync(order, cancellationToken);

            if (paymentResponse is null)
            {
                order.UpdateAsPaymentFailed(
                    async (orderStatusHistory) => await unitOfWork.AddAsync(orderStatusHistory)
                    );
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return;
            }


            order.SetPaymentDetails(
                paymentResponse.Id,
                paymentResponse.RedirectUri,
                async (orderStatusHistory) => await unitOfWork.AddAsync(orderStatusHistory)
                );
        }

        Notification? notification = null;

        if (order.User is RegisteredUser)
        {
            notification = new Notification(
               registeredUserId: order.UserId,
               notificationType: NotificationType.Order,
               message: $"The {nameof(Order)} has been created.",
               resourceId: order.Id.ToString()
               );

            await unitOfWork.AddAsync(notification, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await orderNotificationsSender.SendAsync(
            order,
            OrderNotificationType.NewOrder,
            notification,
            cancellationToken: cancellationToken
            );
    }

    public Task Consume(ConsumeContext<OrderHasBeenCreated> context)
        => HandleAsync(context.Message);
}
