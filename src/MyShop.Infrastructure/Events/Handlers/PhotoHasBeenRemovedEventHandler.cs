using MassTransit;
using MyShop.Application.Abstractions;
using MyShop.Application.Events;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Photos;

namespace MyShop.Infrastructure.Events.Handlers;
internal sealed class PhotoHasBeenRemovedEventHandler(
    IUnitOfWork unitOfWork,
    IPhotoFileService photoFileService
    ) : IEventHandler<PhotoHasBeenRemoved>,
        IConsumer<PhotoHasBeenRemoved>
{
    public async Task HandleAsync(PhotoHasBeenRemoved @event, CancellationToken cancellationToken = default)
    {
        var photo = await unitOfWork.PhotoRepository.GetByIdAsync(
            id: @event.Id,
            cancellationToken: CancellationToken.None
            ) ?? throw new NotFoundException(
                $"Not found {nameof(Photo)} '{@event.Id}' while process {nameof(PhotoHasBeenRemovedEventHandler)}."
                );

        await (photo switch
        {
            ProductVariantPhoto p => RemoveProductVariantPhotoAsync(p),
            UserPhoto p => RemoveUserPhotoAsync(p),
            _ => throw new NotImplementedException($"Not implemented way to removed {nameof(Photo)} with {photo.PhotoType}.")
        });
    }

    private async Task<bool> RemoveProductVariantPhotoAsync(
        ProductVariantPhoto photo
        )
    {
        var photoInUse = await unitOfWork.ProductVariantPhotoItemRepository.AnyAsync(
            e => e.ProductVariantPhotoId.Equals(photo.Id)
            );

        return !photoInUse && await photoFileService.DeletePhotoAsync(photo.FilePath);
    }

    private Task<bool> RemoveUserPhotoAsync(
        UserPhoto photo
        ) => photoFileService.DeletePhotoAsync(photo.FilePath);

    public Task Consume(ConsumeContext<PhotoHasBeenRemoved> context)
        => HandleAsync(context.Message);
}
