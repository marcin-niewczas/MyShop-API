using Microsoft.EntityFrameworkCore;
using MyShop.Application.Abstractions;
using MyShop.Application.Events;
using MyShop.Core.Abstractions.Repositories;

namespace MyShop.Infrastructure.Events.Handlers;
internal sealed class ProductVariantOptionSortTypeHasBeenChangedToAlphabeticallyEventHandler(
    IUnitOfWork unitOfWork
    ) : IEventHandler<ProductVariantOptionSortTypeHasBeenChangedToAlphabetically>
{
    public async Task HandleAsync(
        ProductVariantOptionSortTypeHasBeenChangedToAlphabetically @event,
        CancellationToken cancellationToken = default
        )
    {
        var productVariants = await unitOfWork
            .ProductVariantRepository
            .GetByPredicateAsync(
                predicate: e => e.ProductVariantOptionValues.Any(v => v.ProductOptionId == @event.ProductOptionId),
                include: i => i.Include(e => e.ProductVariantOptionValues)
                               .Include(e => e.Product)
                               .ThenInclude(e => e.ProductProductVariantOptions.OrderBy(o => o.Position)),
                withTracking: true,
                cancellationToken: cancellationToken
                );

        foreach (var variant in productVariants)
        {
            variant.RebuildSortPriority();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
