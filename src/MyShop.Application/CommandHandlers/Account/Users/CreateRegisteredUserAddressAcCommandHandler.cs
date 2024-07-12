using MyShop.Application.Abstractions;
using MyShop.Application.Commands.Account.Users;
using MyShop.Application.Dtos.Account.Users;
using MyShop.Application.Mappings;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Users;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.Users;

namespace MyShop.Application.CommandHandlers.Account.Users;
internal sealed class CreateRegisteredUserAddressAcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateRegisteredUserAddressAc, ApiResponse<UserAddressAcDto>>
{
    public async Task<ApiResponse<UserAddressAcDto>> HandleAsync(
        CreateRegisteredUserAddressAc command, 
        CancellationToken cancellationToken = default
        )
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var userExist = await unitOfWork.RegisteredUserRepository.AnyAsync(e => e.Id == userId, cancellationToken);

        if (!userExist)
        {
            throw new ServerException($"The {nameof(User)} with '{userId}' not exist.");
        }

        var selfAddressCount = await unitOfWork.UserAddressRepository.CountAsync(
                predicate: e => e.RegisteredUserId == userId,
                cancellationToken: cancellationToken
                );

        if (selfAddressCount is >= UserAddress.MaxCountUserAddresses)
        {
            throw new BadRequestException(
                $"Cannot add a {nameof(UserAddress).ToTitleCase()}, beacuse maximum count equal {UserAddress.MaxCountUserAddresses}."
                );
        }

        var userAddressWithTypeExist = await unitOfWork.UserAddressRepository.AnyAsync(
                predicate: e => e.RegisteredUserId == userId && Convert.ToString(e.UserAddressName).ToLower().Equals(command.UserAddressName.ToLower()),
                cancellationToken: cancellationToken
                );

        if (userAddressWithTypeExist)
        {
            throw new BadRequestException($"The {nameof(UserAddress).ToTitleCase()} with {nameof(UserAddressName)} '{command.UserAddressName}' exist.");
        }

        var address = new UserAddress(
            command.StreetName,
            command.BuildingNumber,
            command.ApartmentNumber,
            command.City,
            command.ZipCode,
            command.Country,
            command.UserAddressName,
            userId
            );

        var defaultUserAddress = await unitOfWork.UserAddressRepository.GetFirstByPredicateAsync(
                predicate: e => e.RegisteredUserId == userId && e.IsDefault,
                cancellationToken: cancellationToken
                );

        if (defaultUserAddress is null)
        {
            address.SetIsDefault(true);
        }

        if (defaultUserAddress is not null && command.IsDefault)
        {
            address.SetIsDefault(true);
            defaultUserAddress.SetIsDefault(false);

            await unitOfWork.UpdateAsync(defaultUserAddress);
        }

        address = await unitOfWork.AddAsync(address, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new(address.ToUserAddressAcDto());
    }
}
