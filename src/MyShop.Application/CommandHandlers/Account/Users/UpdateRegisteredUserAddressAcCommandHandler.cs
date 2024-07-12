using MyShop.Application.Abstractions;
using MyShop.Application.Commands.Account.Users;
using MyShop.Application.Dtos.Account.Users;
using MyShop.Application.Mappings;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Users;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Application.CommandHandlers.Account.Users;
internal sealed class UpdateRegisteredUserAddressAcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateRegisteredUserAddressAc, ApiResponse<UserAddressAcDto>>
{
    public async Task<ApiResponse<UserAddressAcDto>> HandleAsync(UpdateRegisteredUserAddressAc command, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var userAddress = await unitOfWork.UserAddressRepository.GetFirstByPredicateAsync(
            predicate: e => e.Id == command.Id && e.RegisteredUserId == userId,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(UserAddress), command.Id);

        if (userAddress.UserAddressName != command.UserAddressName)
        {
            var result = await unitOfWork.UserAddressRepository.AnyAsync(
                predicate: e => e.RegisteredUserId == userId && Convert.ToString(e.UserAddressName).ToLower().Equals(command.UserAddressName.ToLower()),
                cancellationToken: cancellationToken
                );

            if (result)
            {
                throw new BadRequestException(
                    $"The {nameof(UserAddress).ToTitleCase()} with {nameof(UserAddressName).ToTitleCase()} '{command.UserAddressName}' exist."
                    );
            }
        }

        if (command.IsDefault && !userAddress.IsDefault)
        {
            var defaultUserAddress = await unitOfWork.UserAddressRepository.GetFirstByPredicateAsync(
                predicate: e => e.RegisteredUserId == userId && e.IsDefault,
                cancellationToken: cancellationToken
                );

            if (defaultUserAddress is not null)
            {
                defaultUserAddress.SetIsDefault(false);

                await unitOfWork.UpdateAsync(defaultUserAddress);
            }
        }

        if (!command.IsDefault && userAddress.IsDefault)
        {
            var newDefaultAddress = (await unitOfWork.UserAddressRepository.GetByPredicateAsync(
                predicate: e => e.RegisteredUserId == userId && e.Id != userAddress.Id,
                sortByKeySelector: e => e.CreatedAt,
                sortDirection: SortDirection.Descendant,
                take: 1,
                cancellationToken: cancellationToken
                )).FirstOrDefault() ?? throw new BadRequestException(
                    $"This {nameof(UserAddress).ToTitleCase()} must be default, because you have only one {nameof(UserAddress).ToTitleCase()}."
                    );


            newDefaultAddress.SetIsDefault(true);
            await unitOfWork.UpdateAsync(newDefaultAddress);
        }

        userAddress.Update(
            command.StreetName,
            command.BuildingNumber,
            command.ApartmentNumber,
            command.City,
            command.ZipCode,
            command.Country,
            command.UserAddressName,
            command.IsDefault
            );

        userAddress = await unitOfWork.UpdateAsync(userAddress);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new(userAddress.ToUserAddressAcDto());
    }
}
