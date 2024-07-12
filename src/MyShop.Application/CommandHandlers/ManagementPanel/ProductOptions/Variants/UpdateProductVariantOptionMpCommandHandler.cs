using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ManagementPanel.ProductOptions.Variants;
using MyShop.Application.Dtos.ManagementPanel.ProductOptions;
using MyShop.Application.Events;
using MyShop.Application.Mappings;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductOptions.Variants;
internal sealed class UpdateProductVariantOptionMpCommandHandler(
    IUnitOfWork unitOfWork,
    IMessageBroker messageBroker
        ) : ICommandHandler<UpdateProductVariantOptionsMp, ApiResponse<ProductOptionMpDto>>
{
    public async Task<ApiResponse<ProductOptionMpDto>> HandleAsync(UpdateProductVariantOptionsMp command, CancellationToken cancellationToken = default)
    {
        var entity = await (command.ProductOptionSortType switch
        {
            ProductOptionSortType.Alphabetically => unitOfWork.ProductVariantOptionRepository.GetByIdAsync(
                id: command.Id,
                includeExpression: i => i.ProductOptionValues,
                cancellationToken: cancellationToken
                ),
            ProductOptionSortType.Custom => unitOfWork.ProductVariantOptionRepository.GetByIdAsync(
                id: command.Id,
                cancellationToken: cancellationToken
                ),
            _ => throw new BadRequestException(AllowedValuesError.Message<ProductOptionSortType>()),
        }) ?? throw new NotFoundException(nameof(ProductVariantOption).ToTitleCase(), command.Id);

        var isExist = await unitOfWork.BaseProductOptionRepository.AnyAsync(
            e => e.Id != command.Id && Convert.ToString(e.Name).ToLower().Equals(command.Name.ToLower()),
            cancellationToken
            );

        if (isExist)
        {
            throw new BadRequestException($"{nameof(ProductVariantOption).ToTitleCase()} with {nameof(ProductVariantOption.Name)} equal '{command.Name}' exist.");
        }

        var orginalSortType = entity.ProductOptionSortType;

        entity.Update(command.Name, command.ProductOptionSortType);
        entity = await unitOfWork.ProductVariantOptionRepository.UpdateAsync(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (orginalSortType == ProductOptionSortType.Custom &&
           entity.ProductOptionSortType == ProductOptionSortType.Alphabetically)
        {
            await messageBroker.PublishAsync(
                new ProductVariantOptionSortTypeHasBeenChangedToAlphabetically(entity.Id)
                );
        }

        return new(entity.ToProductOptionMpDto());
    }
}
