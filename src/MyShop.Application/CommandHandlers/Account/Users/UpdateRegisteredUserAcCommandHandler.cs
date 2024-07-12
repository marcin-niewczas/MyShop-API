using MyShop.Application.Abstractions;
using MyShop.Application.Commands.Account.Users;
using MyShop.Application.Mappings;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Dtos.Auth;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Users;

namespace MyShop.Application.CommandHandlers.Account.Users;
internal sealed class UpdateRegisteredUserAcCommandHandler(
    IUnitOfWork unitOfWork,
    IUserClaimsService userClaimsService
    ) : ICommandHandler<UpdateRegisteredUserAc, ApiResponse<UserDto>>
{
    public async Task<ApiResponse<UserDto>> HandleAsync(UpdateRegisteredUserAc command, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var user = await unitOfWork.RegisteredUserRepository.GetByIdAsync(
             id: userId,
             withTracking: true,
             cancellationToken: cancellationToken
            ) ?? throw new ServerException($"The {nameof(User)} with '{userId}' not exist.");

        user.Update(
            command.FirstName,
            command.LastName,
            command.Gender,
            command.DateOfBirth,
            command.PhoneNumber
            );

        await unitOfWork.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new(user.ToUserDto());
    }
}
