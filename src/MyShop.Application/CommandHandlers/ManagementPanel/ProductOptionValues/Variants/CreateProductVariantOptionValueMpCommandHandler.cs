using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ManagementPanel.ProductOptionValues.Variants;
using MyShop.Application.Dtos.ManagementPanel.ProductOptionValues;
using MyShop.Application.Events;
using MyShop.Application.Mappings;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductOptionValues.Variants;
internal sealed class CreateProductVariantOptionValueMpCommandHandler(
    IUnitOfWork unitOfWork,
    IMessageBroker messageBroker
    ) : ICommandHandler<CreateProductVariantOptionValueMp, ApiResponse<ProductOptionValueMpDto>>
{
    public async Task<ApiResponse<ProductOptionValueMpDto>> HandleAsync(CreateProductVariantOptionValueMp command, CancellationToken cancellationToken = default)
    {
        var productVariantOptionValue = new ProductVariantOptionValue(command.Value, command.ProductVariantOptionId);

        var productVariantOption = await unitOfWork.ProductVariantOptionRepository.GetByIdAsync(
             id: command.ProductVariantOptionId,
             includeExpression: i => i.ProductOptionValues.OrderBy(v => v.Position),
             withTracking: true,
             cancellationToken: cancellationToken
             ) ?? throw new NotFoundException(nameof(ProductVariantOption), command.ProductVariantOptionId);

        productVariantOption.AddProductVariantOptionValue(productVariantOptionValue);

        productVariantOptionValue = await unitOfWork.ProductVariantOptionValueRepository.AddAsync(
            productVariantOptionValue,
            cancellationToken
            );

        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (productVariantOption.ProductOptionSortType == ProductOptionSortType.Alphabetically)
        {
            await messageBroker.PublishAsync(
                new ProductVariantOptionValueHasBeenAddedInAlphabeticallyProductVariantOption(productVariantOption.Id)
                );
        }

        return new(productVariantOptionValue.ToProductOptionValueMpDto());
    }
}
