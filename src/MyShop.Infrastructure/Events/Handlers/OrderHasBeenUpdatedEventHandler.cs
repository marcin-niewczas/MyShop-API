using MyShop.Application.Abstractions;
using MyShop.Application.Events;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Notifications;
using MyShop.Core.Models.Orders;
using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.Notifications;
using MyShop.Infrastructure.Notifications.Orders;
using MyShop.Infrastructure.Notifications.Senders.Interfaces;

namespace MyShop.Infrastructure.Events.Handlers;
internal sealed class OrderHasBeenUpdatedEventHandler(
    IUnitOfWork unitOfWork,
    IOrderNotificationsSender orderNotificationsSender
    ) : IEventHandler<OrderHasBeenUpdated>
{
    public async Task HandleAsync(OrderHasBeenUpdated @event, CancellationToken cancellationToken = default)
    {
        var order = await unitOfWork.OrderRepository.GetByIdAsync(
            id: @event.OrderId,
            includeExpression: i => i.User,
            cancellationToken: cancellationToken
            );

        if (order is not null)
        {
            Notification? notification = null;

            if (order.User is RegisteredUser)
            {
                notification = new Notification(
                    notificationType: NotificationType.Order,
                    message: $"The {nameof(Order)} has been updated.",
                    registeredUserId: order.UserId,
                    resourceId: order.Id.ToString()
                    );

                await unitOfWork.AddAsync(notification, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

            await orderNotificationsSender.SendAsync(
                order,
                OrderNotificationType.UpdateStatus,
                notification,
                cancellationToken: cancellationToken
                );
        }
    }
}
