using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ManagementPanel.ProductOptionValues.Details;
using MyShop.Application.Dtos.ManagementPanel.ProductOptionValues;
using MyShop.Application.Events;
using MyShop.Application.Mappings;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductOptionValues.Details;
internal sealed class UpdateProductDetailOptionValueMpCommandHandler(
    IUnitOfWork unitOfWork,
    IMessageBroker messageBroker
    ) : ICommandHandler<UpdateProductDetailOptionValueMp, ApiResponse<ProductOptionValueMpDto>>
{
    public async Task<ApiResponse<ProductOptionValueMpDto>> HandleAsync(UpdateProductDetailOptionValueMp command, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.ProductDetailOptionRepository.GetFirstByPredicateAsync(
            predicate: e => e.ProductOptionValues.Any(v => v.Id == command.Id),
            includeExpression: i => i.ProductOptionValues.OrderBy(v => v.Position),
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductVariantOption), command.Id);

        ProductOptionValue? oldValue = null;

        if (entity.ProductOptionSubtype == ProductOptionSubtype.Main)
        {
            oldValue = entity.ProductOptionValues.First(v => v.Id == command.Id).Value;
        }

        var value = entity.UpdateProductDetailOptionValue(command.Id, command.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (oldValue is not null)
        {
            await messageBroker.PublishAsync(new ValueOfMainProductDetailOptionValueHasBeenUpdated(command.Id, oldValue));
        }

        return new(value.ToProductOptionValueMpDto());
    }
}
