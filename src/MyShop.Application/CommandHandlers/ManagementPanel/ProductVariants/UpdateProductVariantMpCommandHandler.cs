using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ManagementPanel.ProductVariants;
using MyShop.Application.Events;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductVariants;
internal sealed class UpdateProductVariantMpCommandHandler(
    IUnitOfWork unitOfWork,
    IMessageBroker messageBroker
    ) : ICommandHandler<UpdateProductVariantMp>
{
    public async Task HandleAsync(UpdateProductVariantMp command, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.ProductVariantRepository.GetByIdAsync(
            id: command.Id,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductVariant), command.Id);

        var orginalQuantity = entity.Quantity;
        var orginalPrice = entity.Price;        

        entity.UpdateQuantity(command.Quantity);
        entity.UpdatePrice(command.Price);

        await unitOfWork.UpdateAsync(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        List<IEvent> messages = [];

        if(orginalPrice > entity.Price)
        {
            messages.Add(new PriceOfTheProductVariantHasBeenReduced(entity.Id));
        }

        if (orginalQuantity <= 0 && entity.Quantity > 0)
        {
            messages.Add(new ProductVariantIsAvailable(entity.Id));
        }

        if (messages.Count > 0) 
        {
            await messageBroker.PublishAsync([.. messages]);
        }
    }
}
