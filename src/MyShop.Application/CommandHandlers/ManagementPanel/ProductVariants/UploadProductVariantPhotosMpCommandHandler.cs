using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ManagementPanel.ProductVariants;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Photos;
using MyShop.Core.Models.Products;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.Products;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductVariants;
internal sealed class UploadProductVariantPhotosMpCommandHandler(
    IUnitOfWork unitOfWork,
    IPhotoFileService imageFileService
    ) : ICommandHandler<UploadProductVariantPhotoMp>
{
    public async Task HandleAsync(UploadProductVariantPhotoMp command, CancellationToken cancellationToken = default)
    {
        var productVariant = await unitOfWork.ProductVariantRepository.GetByIdAsync(
            id: command.Id,
            includeExpression: i => i.PhotoItems.OrderBy(o => o.Position),
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductVariant), command.Id);

        var maxCount = ProductVariantPhotoItemPosition.Max + 1;

        if (productVariant.PhotoItems.Count + command.ProductVariantPhotos.Count > maxCount)
        {
            throw new BadRequestException($"Each {nameof(ProductVariant)} can have max. {maxCount} {nameof(Photo)}s.");
        }

        List<string> imageFilePaths = [];

        try
        {
            var fileDetails = imageFileService.SaveProductVariantPhotosAsync(
                formFileCollection: command.ProductVariantPhotos,
                photoName: productVariant.EncodedName,
                cancellationToken: CancellationToken.None
            );

            var tempPhotoItems = new List<ProductVariantPhotoItem>();

            await foreach (var fileDetail in fileDetails)
            {
                imageFilePaths.Add(fileDetail.FilePath);

                tempPhotoItems.Add(productVariant.AddProductVariantPhotoItem(
                    fileDetail.Position!,
                    new ProductVariantPhoto(
                        name: fileDetail.FileName,
                        photoExtension: fileDetail.FileExtension,
                        contentType: fileDetail.ContentType,
                        photoSize: fileDetail.FileSize,
                        filePath: fileDetail.FilePath,
                        uri: fileDetail.Uri,
                        alt: productVariant.EncodedName.ToTitleCase()
                        )
                    ));
            }

            await unitOfWork.AddRangeAsync(tempPhotoItems, CancellationToken.None);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
        catch (Exception)
        {
            await imageFileService.DeletePhotoAsync(imageFilePaths);
            throw;
        }
    }
}
