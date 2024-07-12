using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ECommerce.ShoppingCarts;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.ShoppingCarts;

namespace MyShop.Application.CommandHandlers.ECommerce.ShoppingCarts;
internal sealed class RemoveShoppingCartItemEcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<RemoveShoppingCartItemEc>
{
    public async Task HandleAsync(RemoveShoppingCartItemEc command, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var shoppingCart = await unitOfWork.ShoppingCartRepository.GetFirstByPredicateAsync(
            predicate: e => e.UserId == userId,
            includeExpression: i => i.ShoppingCartItems,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException($"Not found {nameof(ShoppingCart)}.");

        shoppingCart.RemoveShoppingCartItem(command.Id);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
