using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ManagementPanel.ProductOptionValues.Variants;
using MyShop.Application.Dtos.ManagementPanel.ProductOptionValues;
using MyShop.Application.Events;
using MyShop.Application.Mappings;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductOptionValues.Variants;
internal sealed class UpdateProductVariantOptionValueMpCommandHandler(
    IUnitOfWork unitOfWork,
    IMessageBroker messageBroker
    ) : ICommandHandler<UpdateProductVariantOptionValueMp, ApiResponse<ProductOptionValueMpDto>>
{
    public async Task<ApiResponse<ProductOptionValueMpDto>> HandleAsync(UpdateProductVariantOptionValueMp command, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.ProductVariantOptionRepository.GetFirstByPredicateAsync(
            predicate: e => e.ProductOptionValues.Any(v => v.Id == command.Id),
            includeExpression: i => i.ProductOptionValues.OrderBy(v => v.Position),
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductVariantOption), command.Id);

        var productVariantOptionValue = entity.UpdateProductVariantOptionValue(command.Id, command.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await messageBroker.PublishAsync(
            new ValueOfProductVariantOptionValueHasBeenUpdated(productVariantOptionValue.Id)
            );

        return new(productVariantOptionValue.ToProductOptionValueMpDto());
    }
}
