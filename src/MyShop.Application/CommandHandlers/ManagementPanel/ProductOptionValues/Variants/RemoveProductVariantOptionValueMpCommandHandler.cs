using MyShop.Application.Commands.ManagementPanel.ProductOptions.Variants;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.Utils;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductOptionValues.Variants;
internal sealed class RemoveProductVariantOptionValueMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<RemoveProductVariantOptionValueMp>
{
    public async Task HandleAsync(RemoveProductVariantOptionValueMp command, CancellationToken cancellationToken = default)
    {
        var productOptionValue = await unitOfWork.ProductVariantOptionValueRepository.GetByIdAsync(
            id: command.Id,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductVariantOptionValue), command.Id);

        var productVariantsCount = await unitOfWork.ProductVariantRepository.CountAsync(
            e => e.ProductVariantOptionValues.Any(v => v.Id == productOptionValue.Id),
            cancellationToken
            );

        if (productVariantsCount > 0)
        {
            throw new BadRequestException(
                $"Cannot remove {nameof(ProductDetailOptionValue).ToTitleCase()} '{productOptionValue.Value}', because it is assigned to {productVariantsCount} {(productVariantsCount > 1) switch
                {
                    true => $"{nameof(ProductVariant).ToTitleCase()}s",
                    _ => nameof(ProductVariant).ToTitleCase()
                }}."
                );
        }

        var valuesToChangePosition = await unitOfWork.ProductVariantOptionValueRepository.GetByPredicateAsync(
            predicate: e => e.Position > productOptionValue.Position,
            withTracking: true,
            cancellationToken: cancellationToken
            );

        foreach (var value in valuesToChangePosition)
        {
            value.UpdatePosition(value.Position - 1);
        }

        await unitOfWork.RemoveAsync(productOptionValue);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
