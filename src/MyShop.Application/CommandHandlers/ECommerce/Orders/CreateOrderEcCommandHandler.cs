using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ECommerce.Orders;
using MyShop.Application.Events;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Orders;
using MyShop.Core.Models.ShoppingCarts;
using MyShop.Core.Models.Users;

namespace MyShop.Application.CommandHandlers.ECommerce.Orders;
internal sealed class CreateOrderEcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork,
    IMessageBroker messageBroker
    ) : ICommandHandler<CreateOrderEc, ApiIdResponse>
{
    public async Task<ApiIdResponse> HandleAsync(CreateOrderEc command, CancellationToken cancellationToken = default)
    {
        var claims = userClaimsService.GetUserClaimsData();

        var user = await unitOfWork.UserRepository.GetUserWithShoppingCartItemsAndProductVariantsAsync(
            userId: claims.UserId,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new InvalidDataInDatabaseException($"Not found {nameof(User)} with {nameof(IEntity.Id)} equal '{claims.UserId}'.");

        if (user.ShoppingCart.ShoppingCartItems.Any(i => i.Quantity > i.ProductVariant.Quantity))
        {
            throw new BadRequestException($"The {nameof(ShoppingCart)} isn't verified.");
        }

        var entity = user switch
        {
            RegisteredUser registeredUser => new Order(
                checkoutId: command.CheckoutId,
                streetName: command.StreetName,
                buildingNumber: command.BuildingNumber,
                apartmentNumber: command.ApartmentNumber,
                city: command.City,
                zipCode: command.ZipCode,
                country: command.Country,
                deliveryMethod: command.DeliveryMethod,
                paymentMethod: command.PaymentMethod,
                registeredUser: registeredUser,
                phoneNumber: command.PhoneNumber
                ),
            Guest guest when command is
            {
                PhoneNumber: not null,
                Email: not null,
                FirstName: not null,
                LastName: not null
            } => new Order(
                checkoutId: command.CheckoutId,
                streetName: command.StreetName,
                buildingNumber: command.BuildingNumber,
                apartmentNumber: command.ApartmentNumber,
                city: command.City,
                zipCode: command.ZipCode,
                country: command.Country,
                deliveryMethod: command.DeliveryMethod,
                paymentMethod: command.PaymentMethod,
                guest: guest,
                email: command.Email,
                firstName: command.FirstName,
                lastName: command.LastName,
                phoneNumber: command.PhoneNumber
                ),

            _ => throw new ServerException("Unexpected exception.")
        };

        entity = await unitOfWork.AddAsync(entity, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await messageBroker.PublishAsync(new OrderHasBeenCreated(entity.Id));

        return new(entity.Id);
    }
}
