using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ECommerce.Orders;
using MyShop.Application.Events;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Orders;

namespace MyShop.Application.CommandHandlers.ECommerce.Orders;
internal sealed class CancelOrderEcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork,
    IMessageBroker messageBroker
    ) : ICommandHandler<CancelOrderEc>
{
    public async Task HandleAsync(CancelOrderEc command, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var entity = await unitOfWork.OrderRepository.GetFirstByPredicateAsync(
            predicate: e => e.Id == command.Id && e.UserId == userId,
            includeExpressions: [i => i.OrderProducts, i => i.OrderStatusHistories],
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException($"Not found your {nameof(Order)} with '{command.Id}' {nameof(IEntity.Id)}.");

        if (!entity.Status.CanBeCancelled())
        {
            throw new BadRequestException($"The {nameof(Order)} cannot be cancel.");
        }

        entity.CancelOrder(async (orderStatusHistory) => await unitOfWork.AddAsync(orderStatusHistory));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await messageBroker.PublishAsync(
            new OrderHasBeenCanceled(entity.Id)
            );
    }
}
