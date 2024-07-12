using MyShop.Application.Commands.ManagementPanel.ProductVariants;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductVariants;
internal sealed class UpdatePositionsOfProductVariantPhotoItemsMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdatePositionsOfProductVariantPhotoItemsMp>
{
    public async Task HandleAsync(
        UpdatePositionsOfProductVariantPhotoItemsMp command,
        CancellationToken cancellationToken = default
        )
    {
        var entity = await unitOfWork.ProductVariantRepository.GetByIdAsync(
            id: command.Id,
            includeExpression: i => i.PhotoItems,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductVariantPhotoItem), command.Id);

        entity.UpdatePositionsOfProductVariantPhotoItem(command.IdPositions);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
