using MyShop.Application.Abstractions;
using MyShop.Application.Commands.Account.Users;
using MyShop.Application.Mappings;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.Auth;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Photos;
using MyShop.Core.Utils;

namespace MyShop.Application.CommandHandlers.Account.Users;
internal sealed class UploadUserPhotoAcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork,
    IPhotoFileService photoFileService
    ) : ICommandHandler<UploadUserPhotoAc, ApiResponse<UserDto>>
{
    public async Task<ApiResponse<UserDto>> HandleAsync(UploadUserPhotoAc command, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var user = await unitOfWork.RegisteredUserRepository.GetByIdAsync(
            id: userId,
            includeExpression: i => i.Photo,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException();

        var fileDetail = await photoFileService.SavePhotoAsync(
            command.UserPhoto,
            cancellationToken: cancellationToken
            );

        var oldPhoto = user.Photo;

        try
        {
            var photo = new UserPhoto(
                    name: fileDetail.FileName,
                    photoExtension: fileDetail.FileExtension,
                    contentType: fileDetail.ContentType,
                    photoSize: fileDetail.FileSize,
                    filePath: fileDetail.FilePath,
                    uri: fileDetail.Uri,
                    alt: $"The {nameof(UserPhoto).ToTitleCase()}.",
                    registeredUser: user
                );

            user.SetPhoto(photo);

            await unitOfWork.AddAsync(photo, CancellationToken.None);

            if (oldPhoto is not null)
            {
                await unitOfWork.RemoveAsync(oldPhoto);
            }

            await unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
        catch
        {
            await photoFileService.DeletePhotoAsync(fileDetail.FilePath);
            throw;
        }

        if (oldPhoto is not null)
        {
            await photoFileService.DeletePhotoAsync(oldPhoto.FilePath);
        }

        return new(user.ToUserDto());
    }
}
