using MyShop.Application.Abstractions;
using MyShop.Application.Commands.Account.Users;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Users;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Core.Utils;

namespace MyShop.Application.CommandHandlers.Account.Users;
internal sealed class RemoveRegisteredUserAddressAcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<RemoveRegisteredUserAddressAc>
{
    public async Task HandleAsync(RemoveRegisteredUserAddressAc command, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var userExist = await unitOfWork.RegisteredUserRepository.AnyAsync(e => e.Id == userId, cancellationToken);

        if (!userExist)
        {
            throw new BadRequestException();
        }

        var userAddress = await unitOfWork.UserAddressRepository.GetByIdAsync(
                id: command.Id,
                cancellationToken: cancellationToken
                ) ?? throw new NotFoundException(nameof(UserAddress), command.Id);

        if (userAddress.IsDefault)
        {
            var otherUserAddresses = await unitOfWork.UserAddressRepository.GetByPredicateAsync(
                predicate: e => e.RegisteredUserId == userId && e.Id != userAddress.Id,
                sortByKeySelector: e => e.CreatedAt,
                sortDirection: SortDirection.Descendant,
                cancellationToken: cancellationToken
                );

            if (!otherUserAddresses.IsNullOrEmpty())
            {
                var newDefaultAddress = otherUserAddresses.FirstOrDefault()
                    ?? throw new NotFoundException($"Not found New Default {nameof(UserAddress).ToTitleCase()}.");


                newDefaultAddress.SetIsDefault(true);

                await unitOfWork.UpdateAsync(newDefaultAddress);
            }
        }

        await unitOfWork.RemoveAsync(userAddress);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
