using MyShop.Application.Commands.ManagementPanel.ProductOptions.Details;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.Utils;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductOptions.Details;
internal sealed class RemoveProductDetailOptionMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<RemoveProductDetailOptionMp>
{
    public async Task HandleAsync(RemoveProductDetailOptionMp command, CancellationToken cancellationToken = default)
    {
        var productOption = await unitOfWork.ProductDetailOptionRepository.GetByIdAsync(
            id: command.Id,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductVariantOption), command.Id);

        var productOptionValuesCount = await unitOfWork.ProductDetailOptionValueRepository.CountAsync(
            e => e.ProductOptionId == command.Id,
            cancellationToken
            );

        if (productOptionValuesCount > 0)
        {
            throw new BadRequestException(
                $"Cannot remove {nameof(ProductDetailOption).ToTitleCase()} '{productOption.Name}', because it has {productOptionValuesCount} {(productOptionValuesCount > 1) switch
                {
                    true => "values",
                    _ => "value"
                }}."
                );
        }

        await unitOfWork.RemoveAsync(productOption);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
