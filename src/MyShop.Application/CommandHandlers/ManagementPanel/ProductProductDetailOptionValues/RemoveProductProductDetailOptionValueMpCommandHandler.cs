using Microsoft.EntityFrameworkCore;
using MyShop.Application.Commands.ManagementPanel.ProductProductDetailOptionValues;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductProductDetailOptionValues;
internal sealed class RemoveProductProductDetailOptionValueMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<RemoveProductProductDetailOptionValueMp>
{
    public async Task HandleAsync(
        RemoveProductProductDetailOptionValueMp command,
        CancellationToken cancellationToken = default
        )
    {
        var entity = await unitOfWork.ProductRepository.GetFirstByPredicateAsync(
            predicate: e => e.ProductProductDetailOptionValues.Any(v => v.Id == command.Id),
            include: i => i.Include(e => e.ProductProductDetailOptionValues)
                           .ThenInclude(e => e.ProductDetailOptionValue)
                           .ThenInclude(e => e.ProductDetailOption),
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductProductDetailOptionValue), command.Id);

        entity.RemoveProductProductDetailOptionValue(command.Id);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
