using Microsoft.Extensions.Logging;
using MyShop.Application.Abstractions;
using MyShop.Application.Events;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Models.Notifications;
using MyShop.Core.Models.Orders;
using MyShop.Core.Models.Products;
using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.Notifications;
using MyShop.Infrastructure.Notifications.Orders;
using MyShop.Infrastructure.Notifications.Senders.Interfaces;

namespace MyShop.Infrastructure.Events.Handlers;
internal sealed class OrderHasBeenCanceledEventHandler(
    IUnitOfWork unitOfWork,
    ILogger<OrderHasBeenCanceledEventHandler> logger,
    IOrderNotificationsSender orderNotificationsSender
    ) : IEventHandler<OrderHasBeenCanceled>
{
    public async Task HandleAsync(OrderHasBeenCanceled @event, CancellationToken cancellationToken = default)
    {
        var orderProducts = await unitOfWork.OrderProductRepository.GetByPredicateAsync(
                predicate: e => e.OrderId == @event.OrderId,
                includeExpression: i => i.ProductVariant,
                cancellationToken: cancellationToken
            );

        var productVariantsIds = orderProducts.Select(op => op.ProductVariant.Id).ToArray();

        logger.LogInformation(
            "Start {RestoreQuantity} for {ProductVariantIds} {ProductVariants}.", $"Restore {nameof(ProductVariant.Quantity)}", productVariantsIds, nameof(Product.ProductVariants)
            );

        foreach (var item in orderProducts)
        {
            item.ProductVariant.RestoreQuantity(item.Quantity);
        }

        await unitOfWork.UpdateAsync(orderProducts.Select(op => op.ProductVariant));

        var order = await unitOfWork.OrderRepository.GetByIdAsync(
            id: @event.OrderId,
            includeExpression: i => i.User,
            cancellationToken: cancellationToken
            );

        NotificationRegisteredUser? notification = null;

        if (order is not null and { User: RegisteredUser })
        {
            notification = new NotificationRegisteredUser(
                notificationType: NotificationType.Order,
                message: $"The {nameof(Order)} status has been changed to {order.Status}.",
                registeredUserId: order.UserId,
                resourceId: order.Id.ToString()
                );

            await unitOfWork.AddAsync(notification, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (order is not null)
        {
            await orderNotificationsSender.SendAsync(
                order,
                OrderNotificationType.UpdateStatus,
                notification,
                cancellationToken: cancellationToken
                );
        }

        logger.LogInformation(
            "Finished {RestoreQuantity} for {ProductVariantIds} {ProductVariants}.", $"Restore {nameof(ProductVariant.Quantity)}", productVariantsIds, nameof(Product.ProductVariants)
            );
    }
}
