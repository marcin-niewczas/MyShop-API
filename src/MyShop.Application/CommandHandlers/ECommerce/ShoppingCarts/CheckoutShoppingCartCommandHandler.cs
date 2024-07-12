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
internal sealed class CheckoutShoppingCartCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<CheckoutShoppingCartEc, ApiResponse<CheckoutEcDto>>
{
    public async Task<ApiResponse<CheckoutEcDto>> HandleAsync(CheckoutShoppingCartEc command, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var shoppingCart = await unitOfWork.ShoppingCartRepository.GetFirstByPredicateAsync(
            predicate: e => e.UserId == userId,
            includeExpression: i => i.ShoppingCartItems,
            cancellationToken: cancellationToken
            ) ?? throw new InvalidDataInDatabaseException($"The {nameof(User)} must have a {nameof(ShoppingCart)}.");

        var isNotValid = await unitOfWork.ShoppingCartRepository.AnyAsync(
            e => e.UserId == userId && e.ShoppingCartItems.Any(i => i.Quantity > i.ProductVariant.Quantity),
            cancellationToken
            );

        if (isNotValid)
        {
            throw new BadRequestException($"The {nameof(ShoppingCart)} is not verified.");
        }

        shoppingCart.SetCheckoutId();

        await unitOfWork.UpdateAsync(shoppingCart);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new(shoppingCart.ToCheckoutEcDto());
    }
}
