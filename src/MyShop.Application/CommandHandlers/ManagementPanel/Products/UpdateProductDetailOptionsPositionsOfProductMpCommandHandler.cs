using Microsoft.EntityFrameworkCore;
using MyShop.Application.Commands.ManagementPanel.Products;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Application.CommandHandlers.ManagementPanel.Products;
internal sealed class UpdateProductDetailOptionsPositionsOfProductMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateProductDetailOptionsPositionsOfProductMp>
{
    public async Task HandleAsync(
        UpdateProductDetailOptionsPositionsOfProductMp command, 
        CancellationToken cancellationToken = default
        )
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(
            id: command.ProductId,
            include: i => i.Include(e => e.ProductProductDetailOptionValues)
                           .ThenInclude(e => e.ProductDetailOptionValue)
                           .ThenInclude(e => e.ProductDetailOption),
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(Product), command.ProductId);

        product.ChangePositionsOfProductProductDetailOptions(command.IdPositions);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
