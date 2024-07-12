using MyShop.Application.Abstractions;
using MyShop.Application.Commands.Auth;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Users;

namespace MyShop.Application.CommandHandlers.Auth;
internal sealed class LogoutAuthCommandHandler(
    IUnitOfWork unitOfWork,
    IUserClaimsService userClaimsService
    ) : ICommandHandler<LogoutAuth>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserClaimsService _userClaimsService = userClaimsService;

    public async Task HandleAsync(LogoutAuth command, CancellationToken cancellationToken = default)
    {
        var userClaimsData = _userClaimsService.GetUserClaimsData();

        var userToken = await _unitOfWork.UserTokenRepository.GetByIdAsync(
             id: userClaimsData.UserTokenId,
             cancellationToken: cancellationToken
             ) ?? throw new InvalidDataInDatabaseException(
                 $"Cannot find {nameof(UserToken)} with '{userClaimsData.UserTokenId}' id for {nameof(User)} with '{userClaimsData.UserId}' id."
                 );

        await _unitOfWork.UserTokenRepository.RemoveAsync(userToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
