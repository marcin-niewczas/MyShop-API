using Microsoft.AspNetCore.Http;
using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ManagementPanel.MainPageSections;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.MainPageSections;
using MyShop.Core.Models.Photos;
using MyShop.Core.Utils;

namespace MyShop.Application.CommandHandlers.ManagementPanel.MainPageSections;
internal sealed class UpdateWebsiteHeroSectionItemMpCommandHandler(
    IUnitOfWork unitOfWork,
    IPhotoFileService photoFileService
    ) : ICommandHandler<UpdateWebsiteHeroSectionItemMp>
{
    public Task HandleAsync(UpdateWebsiteHeroSectionItemMp command, CancellationToken cancellationToken = default)
        => command switch
        {
            { WebsiteHeroSectionItemPhoto: not null, WebsiteHeroSectionPhotoId: null }
                => UpdateWebsiteHeroSectionItemWithUploadedPhotoAsync(command, command.WebsiteHeroSectionItemPhoto, cancellationToken),
            { WebsiteHeroSectionItemPhoto: null, WebsiteHeroSectionPhotoId: Guid photoId }
                => UpdateWebsiteHeroSectionItemWithExistingPhotoAsync(command, photoId, cancellationToken),
            _ => throw new NotImplementedException()
        };

    private async Task UpdateWebsiteHeroSectionItemWithUploadedPhotoAsync(
        UpdateWebsiteHeroSectionItemMp command,
        IFormFile photoFile,
        CancellationToken cancellationToken = default
        )
    {
        var websiteHeroSectionItem = await unitOfWork.WebsiteHeroSectionItemRepository.GetByIdAsync(
            id: command.Id,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(WebsiteHeroSectionItem), command.Id);

        var fileDetail = await photoFileService.SavePhotoAsync(
            photoFile,
            cancellationToken: cancellationToken
            );

        try
        {
            var websiteHeroSectionPhoto = new WebsiteHeroSectionPhoto(
                fileDetail.FileName,
                fileDetail.FileExtension,
                fileDetail.ContentType,
                fileDetail.FileSize,
                fileDetail.FilePath,
                fileDetail.Uri,
                $"{nameof(WebsiteHeroSection).ToTitleCase()} Item"
                );

            websiteHeroSectionItem.Update(
                title: command.Title,
                subtitle: command.Subtitle,
                routerLink: command.RouterLink,
                websiteHeroSectionPhoto: websiteHeroSectionPhoto
                );

            await unitOfWork.AddAsync(websiteHeroSectionPhoto, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await photoFileService.DeletePhotoAsync(fileDetail.FilePath);
            throw;
        }
    }

    private async Task UpdateWebsiteHeroSectionItemWithExistingPhotoAsync(
        UpdateWebsiteHeroSectionItemMp command,
        Guid photoId,
        CancellationToken cancellationToken = default
        )
    {
        var websiteHeroSectionItem = await unitOfWork.WebsiteHeroSectionItemRepository.GetByIdAsync(
            id: command.Id,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(WebsiteHeroSection), command.Id);

        var websiteHeroSectionPhoto = await unitOfWork.WebsiteHeroSectionPhotoRepository.GetByIdAsync(
            id: photoId,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(WebsiteHeroSectionPhoto), photoId); ;

        websiteHeroSectionItem.Update(
                title: command.Title,
                subtitle: command.Subtitle,
                routerLink: command.RouterLink,
                websiteHeroSectionPhoto: websiteHeroSectionPhoto
                );

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
