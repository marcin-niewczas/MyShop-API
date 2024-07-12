using Microsoft.EntityFrameworkCore;
using MyShop.Application.Abstractions;
using MyShop.Application.Events;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.ValueObjects.ProductOptions;
using MyShop.Core.ValueObjects.Products;

namespace MyShop.Infrastructure.Events.Handlers;
internal sealed class ValueOfProductVariantOptionValueHasBeenUpdatedEventHandler(
    IUnitOfWork unitOfWork
    ) : IEventHandler<ValueOfProductVariantOptionValueHasBeenUpdated>
{
    public async Task HandleAsync(ValueOfProductVariantOptionValueHasBeenUpdated @event, CancellationToken cancellationToken = default)
    {
        var productVariants = await unitOfWork.ProductVariantRepository.GetByPredicateAsync(
            predicate: e => e.ProductVariantOptionValues.Any(v => v.Id == @event.ProductVariantOptionValueId),
            include: i => i.Include(e => e.ProductVariantOptionValues)
                           .ThenInclude(e => e.ProductVariantOption)
                           .Include(e => e.Product)
                           .ThenInclude(e => e.ProductProductVariantOptions.OrderBy(o => o.Position))
                           .Include(e => e.Product)
                           .ThenInclude(e => e.ProductDetailOptionValues.Where(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main))
                           .ThenInclude(e => e.ProductDetailOption),
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductDetailOptionValue), @event.ProductVariantOptionValueId);

        var productVariantOptionValue = productVariants.First().ProductVariantOptionValues.First(v => v.Id == @event.ProductVariantOptionValueId);

        bool anyChange = false;

        if (productVariantOptionValue.ProductVariantOption.ProductOptionSortType == ProductOptionSortType.Alphabetically)
        {
            foreach (var variant in productVariants)
            {
                if (productVariantOptionValue.ProductVariantOption.ProductOptionSubtype == ProductOptionSubtype.Additional &&
                    variant.Product.DisplayProductType == DisplayProductType.MainVariantOption)
                {
                    variant.RebuildSortPriority();
                    anyChange = true;
                }
                else
                {
                    variant.RebuildSortPriorityAndEncodedName();
                    anyChange = true;
                }
            }
        }
        else
        {
            foreach (var variant in productVariants)
            {
                if (productVariantOptionValue.ProductVariantOption.ProductOptionSubtype == ProductOptionSubtype.Additional &&
                    variant.Product.DisplayProductType == DisplayProductType.MainVariantOption)
                {
                    continue;
                }

                variant.RebuildEncodedName();
                anyChange = true;

            }
        }

        if (anyChange)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
