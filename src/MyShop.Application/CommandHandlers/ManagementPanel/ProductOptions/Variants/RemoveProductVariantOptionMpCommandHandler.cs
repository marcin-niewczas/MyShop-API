using MyShop.Application.Commands.ManagementPanel.ProductOptions.Variants;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.Utils;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductOptions.Variants;
internal sealed class RemoveProductVariantOptionMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<RemoveProductVariantOptionMp>
{
    public async Task HandleAsync(RemoveProductVariantOptionMp command, CancellationToken cancellationToken = default)
    {
        var productOption = await unitOfWork.ProductVariantOptionRepository.GetByIdAsync(
            id: command.Id,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductVariantOption), command.Id);

        var productOptionValuesCount = await unitOfWork.ProductVariantOptionValueRepository.CountAsync(
            e => e.ProductOptionId == command.Id,
            cancellationToken
            );

        if (productOptionValuesCount > 0)
        {
            throw new BadRequestException(
                $"Cannot remove {nameof(ProductVariantOption).ToTitleCase()} '{productOption.Name}', because it has {productOptionValuesCount} {(productOptionValuesCount > 1) switch
                {
                    true => "values",
                    _ => "value"
                }}."
                );
        }

        var productOptionAssignedToProductCount = await unitOfWork.ProductRepository.CountAsync(
            e => e.ProductVariantOptions.Any(o => o.Id == productOption.Id),
            cancellationToken
            );

        if (productOptionAssignedToProductCount > 0)
        {
            throw new BadRequestException(
                $"Cannot remove {nameof(ProductVariantOption).ToTitleCase()} '{productOption.Name}', because it's assigned to {productOptionAssignedToProductCount} {(productOptionAssignedToProductCount > 1) switch
                {
                    true => $"{nameof(Product)}",
                    _ => $"{nameof(Product)}s"
                }}."
                );
        }

        await unitOfWork.RemoveAsync(productOption);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
