using Microsoft.EntityFrameworkCore;
using MyShop.Application.Abstractions;
using MyShop.Application.Events;
using MyShop.Core.Abstractions.Repositories;

namespace MyShop.Infrastructure.Events.Handlers;
internal sealed class PositionsOfProductVariantOptionValuesHasBeenChangedEventHandler(
    IUnitOfWork unitOfWork
    ) : IEventHandler<PositionsOfProductVariantOptionValuesHasBeenChanged>
{
    public async Task HandleAsync(
        PositionsOfProductVariantOptionValuesHasBeenChanged @event,
        CancellationToken cancellationToken = default
        )
    {
        var valueIds = @event.PositionOfProductOptionValue.Select(v => v.Value);

        var productVariants = await unitOfWork
            .ProductVariantRepository
            .GetByPredicateAsync(
                predicate: e => e.ProductVariantOptionValues.Any(v => valueIds.Contains(v.Id)),
                include: i => i.Include(e => e.ProductVariantOptionValues).Include(e => e.Product).ThenInclude(e => e.ProductProductVariantOptions.OrderBy(o => o.Position)),
                withTracking: true,
                cancellationToken: cancellationToken
                );

        foreach (var variant in productVariants)
        {
            variant.UpdateSortPriority(
                @event.PositionOfProductOptionValue
                    .Where(p => variant.ProductVariantOptionValues.Any(v => v.Id == p.Value))
                    .ToArray()
                );

            await unitOfWork.UpdateAsync(variant);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
