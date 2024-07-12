using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ECommerce.Products;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Application.CommandHandlers.ECommerce.Products;
internal sealed class CreateFavoriteEcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateFavoriteEc>
{
    public async Task HandleAsync(CreateFavoriteEc command, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var isExist = await unitOfWork.FavoriteRepository.AnyAsync(
            e => e.RegisteredUserId == userId && e.EncodedProductVariantName == command.EncodedName,
            cancellationToken
            );

        if (isExist)
        {
            throw new BadRequestException($"The {nameof(Favorite)} exist with {nameof(command.EncodedName)} equal '{command.EncodedName}'.");
        }

        var favorite = new Favorite(command.EncodedName, userId);

        await unitOfWork.AddAsync(favorite, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
