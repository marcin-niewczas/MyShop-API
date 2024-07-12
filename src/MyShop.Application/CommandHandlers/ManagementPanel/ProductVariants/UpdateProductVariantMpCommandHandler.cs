using MyShop.Application.Commands.ManagementPanel.ProductVariants;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductVariants;
internal sealed class UpdateProductVariantMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateProductVariantMp>
{
    public async Task HandleAsync(UpdateProductVariantMp command, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.ProductVariantRepository.GetByIdAsync(
            id: command.Id,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductVariant), command.Id);

        entity.UpdateQuantity(command.Quantity);
        entity.UpdatePrice(command.Price);

        await unitOfWork.UpdateAsync(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
