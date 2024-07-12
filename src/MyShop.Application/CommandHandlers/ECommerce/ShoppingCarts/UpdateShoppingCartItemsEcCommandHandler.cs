using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ECommerce.ShoppingCarts;
using MyShop.Application.Dtos.ECommerce.ShoppingCarts;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.ShoppingCarts;
using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.ShoppingCarts;

namespace MyShop.Application.CommandHandlers.ECommerce.ShoppingCarts;
internal sealed class UpdateShoppingCartItemsEcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateShoppingCartItemsEc, ApiResponse<ShoppingCartIdValueDictionaryEcDto>>
{
    public async Task<ApiResponse<ShoppingCartIdValueDictionaryEcDto>> HandleAsync(UpdateShoppingCartItemsEc command, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var shoppingCart = await unitOfWork.ShoppingCartRepository.GetUserShoppingCartWithItemsAndProductVariantsAsync(
            userId: userId,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new ServerException($"Not found {nameof(ShoppingCart)} for {nameof(User)} with {nameof(IEntity.Id)} '{userId}'.");

        Dictionary<Guid, int> errorMaxQuantityDictionary = [];
        ShoppingCartItemUpdateResult tempResult;
        int countResultNoUpdate = 0;

        foreach (var updateModel in command)
        {
            tempResult = shoppingCart
                .UpdateShoppingCartItem(updateModel.ShoppingCartItemId, updateModel.Quantity);

            switch (tempResult.ShoppingCartItemUpdateState)
            {
                case ShoppingCartItemUpdateState.NoUpdated:
                    countResultNoUpdate++;
                    break;
                case ShoppingCartItemUpdateState.UpdatedToMaxQuantityInStock:
                    errorMaxQuantityDictionary[tempResult.ShoppingCartItem.Id] = tempResult.ShoppingCartItem.ProductVariant.Quantity;
                    break;
                case ShoppingCartItemUpdateState.UpdatedToMaxQuantityInShoppingCart:
                    errorMaxQuantityDictionary[tempResult.ShoppingCartItem.Id] = ShoppingCartItemQuantity.Max;
                    break;
            }
        }

        if (countResultNoUpdate == command.Count)
        {
            throw new BadRequestException($"Nothing change in {nameof(ShoppingCart)}.");
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new(new(errorMaxQuantityDictionary));
    }
}
