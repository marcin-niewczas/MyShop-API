using Microsoft.AspNetCore.Http;
using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ManagementPanel.MainPageSections;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.MainPageSections;
using MyShop.Core.Models.Photos;
using MyShop.Core.Utils;

namespace MyShop.Application.CommandHandlers.ManagementPanel.MainPageSections;
internal sealed class CreateWebsiteHeroSectionItemMpCommandHandler(
    IUnitOfWork unitOfWork,
    IPhotoFileService photoFileService
    ) : ICommandHandler<CreateWebsiteHeroSectionItemMp, ApiIdResponse>
{
    public async Task<ApiIdResponse> HandleAsync(CreateWebsiteHeroSectionItemMp command, CancellationToken cancellationToken = default)
    {
        var createdEntity = await (command switch
        {
            { WebsiteHeroSectionItemPhoto: not null, WebsiteHeroSectionPhotoId: null, Position: int }
                => CreateActiveWebsiteHeroSectionItemWithUploadedPhotoAsync(command, command.WebsiteHeroSectionItemPhoto, cancellationToken),
            { WebsiteHeroSectionItemPhoto: not null, WebsiteHeroSectionPhotoId: null, Position: null }
                => CreateInactiveWebsiteHeroSectionItemWithUploadedPhotoAsync(command, command.WebsiteHeroSectionItemPhoto, cancellationToken),
            { WebsiteHeroSectionItemPhoto: null, WebsiteHeroSectionPhotoId: Guid photoId, Position: int }
                => CreateActiveWebsiteHeroSectionItemWithExistingPhotoAsync(command, photoId, cancellationToken),
            { WebsiteHeroSectionItemPhoto: null, WebsiteHeroSectionPhotoId: Guid photoId, Position: null }
                => CreateInactiveWebsiteHeroSectionItemWithExistingPhotoAsync(command, photoId, cancellationToken),
            _ => throw new NotImplementedException()
        });

        return new(createdEntity.Id);
    }


    private async Task<WebsiteHeroSectionItem> CreateActiveWebsiteHeroSectionItemWithUploadedPhotoAsync(
        CreateWebsiteHeroSectionItemMp command,
        IFormFile photoFile,
        CancellationToken cancellationToken = default
        )
    {
        var websiteHeroSection = await unitOfWork.WebsiteHeroSectionRepository.GetByIdAsync(
            id: command.WebsiteHeroSectionId,
            includeExpression: i => i.WebsiteHeroSectionItems.Where(e => e.Position != null).OrderBy(o => o.Position),
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(WebsiteHeroSection), command.WebsiteHeroSectionId);

        var fileDetail = await photoFileService.SavePhotoAsync(
            photoFile,
            cancellationToken: cancellationToken
            );

        WebsiteHeroSectionItem websiteHeroSectionItem;

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

            websiteHeroSectionItem = new WebsiteHeroSectionItem(
                websiteHeroSection,
                websiteHeroSectionPhoto,
                command.Position,
                command.Title,
                command.Subtitle,
                command.RouterLink
                );

            websiteHeroSection.AddItem(websiteHeroSectionItem);

            await unitOfWork.AddAsync(websiteHeroSectionItem, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await photoFileService.DeletePhotoAsync(fileDetail.FilePath);
            throw;
        }

        return websiteHeroSectionItem;
    }

    private async Task<WebsiteHeroSectionItem> CreateInactiveWebsiteHeroSectionItemWithUploadedPhotoAsync(
        CreateWebsiteHeroSectionItemMp command,
        IFormFile photoFile,
        CancellationToken cancellationToken = default
        )
    {
        var websiteHeroSection = await unitOfWork.WebsiteHeroSectionRepository.GetByIdAsync(
            id: command.WebsiteHeroSectionId,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(WebsiteHeroSection), command.WebsiteHeroSectionId);

        var fileDetail = await photoFileService.SavePhotoAsync(
            photoFile,
            cancellationToken: cancellationToken
            );

        WebsiteHeroSectionItem websiteHeroSectionItem;

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

            websiteHeroSectionItem = new WebsiteHeroSectionItem(
                websiteHeroSection,
                websiteHeroSectionPhoto,
                command.Position,
                command.Title,
                command.Subtitle,
                command.RouterLink
                );

            await unitOfWork.AddAsync(websiteHeroSectionItem, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await photoFileService.DeletePhotoAsync(fileDetail.FilePath);
            throw;
        }

        return websiteHeroSectionItem;
    }

    private async Task<WebsiteHeroSectionItem> CreateActiveWebsiteHeroSectionItemWithExistingPhotoAsync(
        CreateWebsiteHeroSectionItemMp command,
        Guid photoId,
        CancellationToken cancellationToken = default
        )
    {
        var websiteHeroSection = await unitOfWork.WebsiteHeroSectionRepository.GetByIdAsync(
            id: command.WebsiteHeroSectionId,
            includeExpression: i => i.WebsiteHeroSectionItems.Where(e => e.Position != null).OrderBy(o => o.Position),
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(WebsiteHeroSection), command.WebsiteHeroSectionId);

        var websiteHeroSectionPhoto = await unitOfWork.WebsiteHeroSectionPhotoRepository.GetByIdAsync(
            id: photoId,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(WebsiteHeroSectionPhoto), photoId); ;

        var websiteHeroSectionItem = new WebsiteHeroSectionItem(
            websiteHeroSection,
            websiteHeroSectionPhoto,
            command.Position,
            command.Title,
            command.Subtitle,
            command.RouterLink
            );

        websiteHeroSection.AddItem(websiteHeroSectionItem);

        await unitOfWork.AddAsync(websiteHeroSectionItem, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return websiteHeroSectionItem;
    }

    private async Task<WebsiteHeroSectionItem> CreateInactiveWebsiteHeroSectionItemWithExistingPhotoAsync(
        CreateWebsiteHeroSectionItemMp command,
        Guid photoId,
        CancellationToken cancellationToken = default
        )
    {
        var websiteHeroSection = await unitOfWork.WebsiteHeroSectionRepository.GetByIdAsync(
            id: command.WebsiteHeroSectionId,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(WebsiteHeroSection), command.WebsiteHeroSectionId);

        var websiteHeroSectionPhoto = await unitOfWork.WebsiteHeroSectionPhotoRepository.GetByIdAsync(
            id: photoId,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(WebsiteHeroSectionPhoto), photoId); ;

        var websiteHeroSectionItem = new WebsiteHeroSectionItem(
            websiteHeroSection,
            websiteHeroSectionPhoto,
            command.Position,
            command.Title,
            command.Subtitle,
            command.RouterLink
            );

        await unitOfWork.AddAsync(websiteHeroSectionItem, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return websiteHeroSectionItem;
    }
}
