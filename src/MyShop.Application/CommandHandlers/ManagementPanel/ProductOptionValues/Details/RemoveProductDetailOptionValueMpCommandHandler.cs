using MyShop.Application.Commands.ManagementPanel.ProductOptions.Details;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.Utils;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductOptionValues.Details;
internal sealed class RemoveProductDetailOptionValueMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<RemoveProductDetailOptionValueMp>
{
    public async Task HandleAsync(RemoveProductDetailOptionValueMp command, CancellationToken cancellationToken = default)
    {
        var productOptionValue = await unitOfWork.ProductDetailOptionValueRepository.GetByIdAsync(
            id: command.Id,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductVariantOptionValue), command.Id);

        var productsCount = await unitOfWork.ProductRepository.CountAsync(
            e => e.ProductDetailOptionValues.Any(v => v.Id == productOptionValue.Id),
            cancellationToken
            );

        if (productsCount > 0)
        {
            throw new BadRequestException(
                $"Cannot remove {nameof(ProductDetailOptionValue).ToTitleCase()} '{productOptionValue.Value}', because it is assigned to {productsCount} {(productsCount > 1) switch
                {
                    true => $"{nameof(Product)}s",
                    _ => nameof(Product)
                }}."
                );
        }

        var valuesToChangePosition = await unitOfWork.ProductDetailOptionValueRepository.GetByPredicateAsync(
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
