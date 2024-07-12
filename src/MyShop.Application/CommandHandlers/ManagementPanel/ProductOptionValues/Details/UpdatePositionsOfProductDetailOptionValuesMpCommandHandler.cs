using MyShop.Application.Commands.ManagementPanel.ProductOptionValues.Details;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductOptionValues.Details;
internal sealed class UpdatePositionsOfProductDetailOptionValuesMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdatePositionsOfProductDetailOptionValuesMp>
{
    public async Task HandleAsync(UpdatePositionsOfProductDetailOptionValuesMp command, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.ProductDetailOptionRepository.GetByIdAsync(
             id: command.ProductDetailOptionId,
             includeExpression: i => i.ProductOptionValues.OrderBy(v => v.Position),
             withTracking: true,
             cancellationToken: cancellationToken
             ) ?? throw new NotFoundException(nameof(ProductVariantOption), command.ProductDetailOptionId);

        entity.UpdatePositionsOfProductDetailOptionValues(command.PositionsOfProductDetailOptionValues);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
