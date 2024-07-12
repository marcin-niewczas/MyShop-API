using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ECommerce.ShoppingCarts;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.Models.ShoppingCarts;

namespace MyShop.Application.CommandHandlers.ECommerce.ShoppingCarts;
internal sealed class CreateShoppingCartItemEcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateShoppingCartItemEc>
{
    public async Task HandleAsync(CreateShoppingCartItemEc command, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var shoppingCart = await unitOfWork.ShoppingCartRepository.GetFirstByPredicateAsync(
            predicate: e => e.UserId == userId,
            includeExpression: i => i.ShoppingCartItems,
            cancellationToken: cancellationToken
            );

        var productVariant = await unitOfWork.ProductVariantRepository.GetByIdAsync(
            id: command.ProductVariantId,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductVariant), command.ProductVariantId);

        if (shoppingCart is null)
        {
            shoppingCart = new ShoppingCart(userId);
            shoppingCart.AddShoppingCartItem(productVariant);

            await unitOfWork.ShoppingCartRepository.AddAsync(shoppingCart, cancellationToken);
        }
        else
        {
            var (item, isUpdated) = shoppingCart.AddShoppingCartItem(productVariant);

            if (isUpdated)
            {
                await unitOfWork.UpdateAsync(item);
            }
            else
            {
                await unitOfWork.AddAsync(item, cancellationToken);
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
