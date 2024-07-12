using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ManagementPanel.Orders;
using MyShop.Application.Events;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Orders;
using MyShop.Core.ValueObjects.Orders;

namespace MyShop.Application.CommandHandlers.ManagementPanel.Orders;
internal sealed class UpdateOrderMpCommandHandler(
    IUnitOfWork unitOfWork,
    IMessageBroker messageBroker
    ) : ICommandHandler<UpdateOrderMp>
{
    public async Task HandleAsync(UpdateOrderMp command, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.OrderRepository.GetByIdAsync(
            id: command.Id,
            includeExpression: i => i.OrderStatusHistories,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(Order), command.Id);

        var orginalStatus = entity.Status;

        var sendNotificationAboutOrderDataChanged = false;

        if (entity.StreetName != command.StreetName ||
            entity.BuildingNumber != command.BuildingNumber ||
            entity.ApartmentNumber != command.ApartmentNumber ||
            entity.City != command.City ||
            entity.ZipCode != command.ZipCode ||
            entity.Country != command.Country ||
            entity.Email != command.Email ||
            entity.FirstName != command.FirstName ||
            entity.LastName != command.LastName ||
            entity.PhoneNumber != command.PhoneNumber)
        {
            sendNotificationAboutOrderDataChanged = true;
        }

        entity.Update(
            streetName: command.StreetName,
            buildingNumber: command.BuildingNumber,
            apartmentNumber: command.ApartmentNumber,
            city: command.City,
            zipCode: command.ZipCode,
            country: command.Country,
            email: command.Email,
            firstName: command.FirstName,
            lastName: command.LastName,
            phoneNumber: command.PhoneNumber,
            orderStatus: command.OrderStatus,
            afterChangeStatus: async (orderStatusHistory) => await unitOfWork.AddAsync(orderStatusHistory, cancellationToken)
            );

        await unitOfWork.UpdateAsync(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (orginalStatus != OrderStatus.Canceled && entity.Status == OrderStatus.Canceled)
        {
            await messageBroker.PublishAsync(new OrderHasBeenCanceled(entity.Id));
        }

        if (sendNotificationAboutOrderDataChanged)
        {
            await messageBroker.PublishAsync(new OrderHasBeenUpdated(entity.Id));
        }
    }
}
