using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ManagementPanel.ProductVariants;
using MyShop.Application.Events;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductVariants;
internal sealed class RemoveProductVariantPhotoItemsMpCommandHandler(
    IUnitOfWork unitOfWork,
    IMessageBroker messageBroker
    ) : ICommandHandler<RemoveProductVariantPhotoItemsMp>
{
    public async Task HandleAsync(RemoveProductVariantPhotoItemsMp command, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.ProductVariantRepository.GetFirstByPredicateAsync(
            predicate: e => e.PhotoItems.Any(p => p.Id == command.Id),
            includeExpression: i => i.PhotoItems,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductVariantPhotoItem), command.Id);

        var toRemove = entity.RemoveProductVariantPhotoItem(command.Id);

        await unitOfWork.RemoveAsync(toRemove);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await messageBroker.PublishAsync(
            new PhotoHasBeenRemoved(toRemove.ProductVariantPhotoId)
            );
    }
}
