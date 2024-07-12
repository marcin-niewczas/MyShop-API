using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ECommerce.ShoppingCarts;
using MyShop.Application.Dtos.ECommerce.ShoppingCarts;
using MyShop.Application.Mappings;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.ShoppingCarts;
using MyShop.Core.Models.Users;

namespace MyShop.Application.CommandHandlers.ECommerce.ShoppingCarts;
internal sealed class VerifyShoppingCartEcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<VerifyShoppingCartEc, ApiResponse<ShoppingCartDetailEcDto>>
{
    public async Task<ApiResponse<ShoppingCartDetailEcDto>> HandleAsync(VerifyShoppingCartEc query, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var shoppingCartVerifier = await unitOfWork.ShoppingCartRepository.GetShoppingCartVerifierAsync(
            userId: userId,
            cancellationToken: cancellationToken
            ) ?? throw new InvalidDataInDatabaseException($"The {nameof(User)} must have a {nameof(ShoppingCart)}.");

        var changed = shoppingCartVerifier.Verify();

        if (changed.Count > 0)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new(shoppingCartVerifier.ToShoppingCartDetailEcDto(changed));
    }
}
