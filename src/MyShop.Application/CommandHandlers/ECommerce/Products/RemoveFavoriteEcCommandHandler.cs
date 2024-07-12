using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ECommerce.Products;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.CommandHandlers.ECommerce.Products;
internal sealed class RemoveFavoriteEcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<RemoveFavoriteEc>
{
    public async Task HandleAsync(RemoveFavoriteEc command, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var favorite = await unitOfWork.FavoriteRepository.GetFirstByPredicateAsync(
            predicate: e => e.RegisteredUserId == userId && e.EncodedProductVariantName == command.EncodedName,
            cancellationToken: cancellationToken
            ) ?? throw new BadRequestException($"The {nameof(Product)} isn't in {nameof(Favorite)}s.");

        await unitOfWork.RemoveAsync(favorite);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
