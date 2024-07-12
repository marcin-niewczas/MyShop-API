using MyShop.Application.Abstractions;
using MyShop.Application.Commands.Account.Users;
using MyShop.Application.Events;
using MyShop.Core.Abstractions;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Users;

namespace MyShop.Application.CommandHandlers.Account.Users;
internal sealed class UpdateRegisteredUserEmailAcCommandHandler(
    IUserClaimsService userClaimsService,
    IPasswordManager passwordManager,
    IUnitOfWork unitOfWork,
    IMessageBroker messageBroker
    ) : ICommandHandler<UpdateRegisteredUserEmailAc>
{
    public async Task HandleAsync(UpdateRegisteredUserEmailAc command, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var user = await unitOfWork.RegisteredUserRepository.GetByIdAsync(
            id: userId,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new ServerException($"Not found {nameof(User)} with {nameof(IEntity.Id)} equal '{userId}'.");

        if (await unitOfWork.RegisteredUserRepository.AnyAsync(e => Convert.ToString(e.Email).ToLower().Equals(command.NewEmail.ToLower()), cancellationToken))
        {
            throw new BadRequestException($"Account with {nameof(RegisteredUser.Email)} equals '{command.NewEmail}' exist.");
        }

        if (!passwordManager.Verify(command.Password, user.SecuredPassword))
        {
            throw new BadRequestException("Invalid password.");
        }

        user.UpdateEmail(command.NewEmail);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await messageBroker.PublishAsync(new AuthEmailHasBeenChanged(userId));
    }
}
