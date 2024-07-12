using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ManagementPanel.ProductOptionValues.Variants;
using MyShop.Application.Events;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductOptionValues.Variants;
internal sealed class UpdatePositionsOfProductVariantOptionValuesMpCommandHandler(
    IUnitOfWork unitOfWork,
    IMessageBroker messageBroker
    ) : ICommandHandler<UpdatePositionsOfProductVariantOptionValuesMp>
{
    public async Task HandleAsync(UpdatePositionsOfProductVariantOptionValuesMp command, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.ProductVariantOptionRepository.GetByIdAsync(
             id: command.ProductVariantOptionId,
             includeExpression: i => i.ProductOptionValues.OrderBy(v => v.Position),
             withTracking: true,
             cancellationToken: cancellationToken
             ) ?? throw new NotFoundException(nameof(ProductVariantOption), command.ProductVariantOptionId);

        entity.UpdatePositionsOfProductVariantOptionValues(command.PositionsOfProductVariantOptionValues);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await messageBroker.PublishAsync(
            new PositionsOfProductVariantOptionValuesHasBeenChanged(command.PositionsOfProductVariantOptionValues)
            );
    }
}
