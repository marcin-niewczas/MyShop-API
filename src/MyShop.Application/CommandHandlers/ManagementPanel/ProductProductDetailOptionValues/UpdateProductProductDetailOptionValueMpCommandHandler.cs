using Microsoft.EntityFrameworkCore;
using MyShop.Application.Commands.ManagementPanel.ProductProductDetailOptionValues;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductProductDetailOptionValues;
internal sealed class UpdateProductProductDetailOptionValueMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateProductProductDetailOptionValueMp>
{
    public async Task HandleAsync(UpdateProductProductDetailOptionValueMp command, CancellationToken cancellationToken = default)
    {
        var productProductDetailOptionValue = await unitOfWork.ProductProductDetailOptionValueRespository.GetByIdAsync(
            id: command.Id,
            include: i => i.Include(e => e.ProductDetailOptionValue).ThenInclude(e => e.ProductDetailOption),
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductProductDetailOptionValue), command.Id);

        var chosenProductDetailOptionValue = await unitOfWork.ProductDetailOptionValueRepository.GetByIdAsync(
            id: command.ProductDetailOptionValueId,
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductDetailOptionValue), command.Id);

        ProductOptionValue? oldValue = null;

        if (productProductDetailOptionValue.ProductDetailOptionValue.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main)
        {
            oldValue = productProductDetailOptionValue.ProductDetailOptionValue.Value;
        }

        productProductDetailOptionValue.UpdateProductDetailOptionValue(
            chosenProductDetailOptionValue
            );

        await unitOfWork.UpdateAsync(productProductDetailOptionValue);

        if (oldValue is not null)
        {
            var productVariants = await unitOfWork.ProductVariantRepository.GetByPredicateAsync(
                predicate: e => e.ProductId == productProductDetailOptionValue.ProductId,
                cancellationToken: cancellationToken
                );

            foreach (var variant in productVariants)
            {
                variant.ReplaceMainDetailOptionValueInEncodedName(
                    oldValue,
                    chosenProductDetailOptionValue.Value
                    );
            }

            await unitOfWork.UpdateAsync(productVariants);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
