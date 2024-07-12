using MyShop.Application.Abstractions;
using MyShop.Application.Commands.Account.Users;
using MyShop.Application.Events;
using MyShop.Application.Mappings;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.Auth;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Photos;

namespace MyShop.Application.CommandHandlers.Account.Users;
internal sealed class RemoveUserPhotoAcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork,
    IMessageBroker messageBroker
    ) : ICommandHandler<RemoveUserPhotoAc, ApiResponse<UserDto>>
{
    public async Task<ApiResponse<UserDto>> HandleAsync(RemoveUserPhotoAc command, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var user = await unitOfWork.RegisteredUserRepository.GetByIdAsync(
            id: userId,
            includeExpression: i => i.Photo,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException();

        var currentPhoto = user.Photo
            ?? throw new BadRequestException($"The {nameof(UserPhoto)} not exist.");

        await unitOfWork.RemoveAsync(currentPhoto);
        await unitOfWork.SaveChangesAsync(CancellationToken.None);

        await messageBroker.PublishAsync(
            new PhotoHasBeenRemoved(currentPhoto.Id)
            );

        user.SetPhoto(null);

        return new(user.ToUserDto());
    }
}
