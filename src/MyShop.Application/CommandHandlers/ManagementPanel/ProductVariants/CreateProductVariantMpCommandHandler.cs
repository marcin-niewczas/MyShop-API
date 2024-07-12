using MyShop.Application.Commands.ManagementPanel.ProductVariants;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Photos;
using MyShop.Core.Models.Products;
using MyShop.Core.Utils;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductVariants;
internal sealed class CreateProductVariantMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateProductVariantMp, ApiIdResponse>
{
    public async Task<ApiIdResponse> HandleAsync(CreateProductVariantMp command, CancellationToken cancellationToken = default)
    {
        var product = await unitOfWork.ProductRepository.GetProductByIdForCreateProductVariantAsync(
            id: command.ProductId,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(Product), command.ProductId);

        var productVariantOptionValues = await unitOfWork.ProductVariantOptionValueRepository.GetSortedProductVariantOptionValuesAsync(
            productId: command.ProductId,
            productVariantOptionValueIds: command.ProductVariantOptionValueIds,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new BadRequestException($"Invalid {nameof(ProductVariantOptionValue).ToTitleCase()} Id/Ids.");

        var isExistProductVariant = await unitOfWork.ProductVariantRepository.AnyAsync(
            predicate: e => e.ProductId == product.Id && e.ProductVariantOptionValues.All(v => command.ProductVariantOptionValueIds.Contains(v.Id)),
            cancellationToken: cancellationToken
            );

        if (isExistProductVariant)
            throw new BadRequestException(
                $"The {nameof(ProductVariant).ToTitleCase()} with introduced {nameof(ProductVariantOptionValue).ToTitleCase()}/{nameof(ProductVariant.ProductVariantOptionValues).ToTitleCase()} is exist."
            );

        ProductVariant productVariant;

        if (command.PhotosIdPositions.IsNullOrEmpty())
        {
            productVariant = new(command.Quantity, command.Price, product, command.ProductId, productVariantOptionValues);
        }
        else
        {
            var photosCount = await unitOfWork.ProductVariantPhotoRepository.CountAsync(
                predicate: e => command.PhotosIdPositions.Select(i => i.Value).Contains(e.Id),
                cancellationToken: cancellationToken
                );

            if (photosCount != command.PhotosIdPositions.Count)
                throw new BadRequestException(command.PhotosIdPositions.Count > 1 ? $"Invalid {nameof(Photo)} Ids." : $"Invalid {nameof(Photo)} Id.");

            productVariant = new(
                command.Quantity,
                command.Price,
                product,
                command.ProductId,
                productVariantOptionValues,
                command.PhotosIdPositions
                );
        }

        await unitOfWork.ProductVariantRepository.AddAsync(productVariant, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new(productVariant.Id);
    }
}
