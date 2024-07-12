using MyShop.Application.Abstractions;
using MyShop.Application.Commands.Account.Users;
using MyShop.Application.Events;
using MyShop.Core.Abstractions;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Users;

namespace MyShop.Application.CommandHandlers.Account.Users;
internal sealed class UpdateRegisteredUserPasswordAcCommandHandler(
    IUserClaimsService userClaimsService,
    IPasswordManager passwordManager,
    IUnitOfWork unitOfWork,
    IMessageBroker messageBroker
    ) : ICommandHandler<UpdateRegisteredUserPasswordAc>
{
    public async Task HandleAsync(
        UpdateRegisteredUserPasswordAc command, 
        CancellationToken cancellationToken = default
        )
    {
        var userClaimsData = userClaimsService.GetUserClaimsData();

        await (command.LogoutOtherDevices switch
        {
            true => UpdateUserPasswordWithLogoutAsync(userClaimsData, command, cancellationToken),
            _ => UpdateUserPasswordWithoutLogoutAsync(userClaimsData, command, cancellationToken)
        });

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await messageBroker.PublishAsync(new AuthPasswordHasBeenChanged(userClaimsData.UserId));
    }


    private async Task UpdateUserPasswordWithoutLogoutAsync(UserClaimsData userClaimsData, UpdateRegisteredUserPasswordAc command, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.RegisteredUserRepository.GetByIdAsync(
            id: userClaimsData.UserId,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new ServerException($"Not found {nameof(User)} with {nameof(IEntity.Id)} equal '{userClaimsData.UserId}'.");

        VerifyAndUpdateUserPassword(user, command);
    }

    private async Task UpdateUserPasswordWithLogoutAsync(UserClaimsData userClaimsData, UpdateRegisteredUserPasswordAc command, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.RegisteredUserRepository.GetByIdAsync(
            id: userClaimsData.UserId,
            includeExpression: i => i.UserTokens,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new ServerException($"Not found {nameof(User)} with {nameof(IEntity.Id)} equal '{userClaimsData.UserId}'.");

        VerifyAndUpdateUserPassword(user, command);
        user.ClearUserTokens(userClaimsData.UserTokenId);
    }

    private void VerifyAndUpdateUserPassword(RegisteredUser user, UpdateRegisteredUserPasswordAc command)
    {
        if (!passwordManager.Verify(command.Password, user.SecuredPassword))
        {
            throw new BadRequestException("Invalid password.");
        }

        var securedNewPassword = passwordManager.SecurePassword(command.NewPassword);
        user.UpdateSecuredPassword(securedNewPassword);
    }
}
