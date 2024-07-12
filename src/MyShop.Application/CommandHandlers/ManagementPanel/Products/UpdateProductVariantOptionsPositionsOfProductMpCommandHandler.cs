using Microsoft.EntityFrameworkCore;
using MyShop.Application.Commands.ManagementPanel.Products;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Application.CommandHandlers.ManagementPanel.Products;
internal sealed class UpdateProductVariantOptionsPositionsOfProductMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateProductVariantOptionsPositionsOfProductMp>
{
    public async Task HandleAsync(UpdateProductVariantOptionsPositionsOfProductMp command, CancellationToken cancellationToken = default)
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(
            id: command.ProductId,
            include: i => i.Include(e => e.ProductVariants)
                           .ThenInclude(e => e.ProductVariantOptionValues)
                           .Include(e => e.ProductProductVariantOptions)
                           .ThenInclude(e => e.ProductVariantOption)
                           .Include(e => e.ProductDetailOptionValues.Where(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main))
                           .ThenInclude(e => e.ProductDetailOption),
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(Product), command.ProductId);

        product.ChangePositionsOfProductProductVariantOptions(command.IdPositions);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
