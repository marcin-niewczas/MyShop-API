using MyShop.Application.Commands.ManagementPanel.ProductVariants;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Photos;
using MyShop.Core.Models.Products;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductVariants;
internal sealed class CreateProductVariantPhotoItemsMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateProductVariantPhotoItemsMp>
{
    public async Task HandleAsync(CreateProductVariantPhotoItemsMp command, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.ProductVariantRepository.GetByIdAsync(
            id: command.Id,
            includeExpression: i => i.PhotoItems,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductVariant), command.Id);

        var chosenPhotoCount = await unitOfWork.ProductVariantPhotoRepository.CountAsync(
            predicate: p => command.IdPositions.Select(ip => ip.Value).Contains(p.Id),
            cancellationToken: cancellationToken
            );

        if (chosenPhotoCount != command.IdPositions.Count)
        {
            throw new BadRequestException(
                $"Invalid {nameof(ProductVariantPhoto)} {nameof(IEntity.Id)}/{nameof(IEntity.Id)}s."
                );
        }

        List<ProductVariantPhotoItem> productVariantPhotoItems = [];

        foreach (var idPosition in command.IdPositions)
        {
            productVariantPhotoItems.Add(entity.AddProductVariantPhotoItem(idPosition));
        }

        await unitOfWork.AddRangeAsync(productVariantPhotoItems, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
