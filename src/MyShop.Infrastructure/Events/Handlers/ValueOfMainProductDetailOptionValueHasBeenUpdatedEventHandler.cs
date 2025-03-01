using MassTransit;
using Microsoft.EntityFrameworkCore;
using MyShop.Application.Abstractions;
using MyShop.Application.Events;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Infrastructure.Events.Handlers;
internal sealed class ValueOfMainProductDetailOptionValueHasBeenUpdatedEventHandler(
    IUnitOfWork unitOfWork
    ) : IEventHandler<ValueOfMainProductDetailOptionValueHasBeenUpdated>,
        IConsumer<ValueOfMainProductDetailOptionValueHasBeenUpdated>
{
    public async Task HandleAsync(
        ValueOfMainProductDetailOptionValueHasBeenUpdated @event,
        CancellationToken cancellationToken = default
        )
    {
        var value = await unitOfWork.ProductDetailOptionValueRepository.GetByIdAsync(
            id: @event.ProductDetailOptionValueId,
            include: i => i.Include(e => e.Products).ThenInclude(e => e.ProductVariants),
            withTracking: true,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductDetailOptionValue), @event.ProductDetailOptionValueId);

        foreach (var product in value.Products)
        {
            foreach (var variant in product.ProductVariants)
            {
                variant.ReplaceMainDetailOptionValueInEncodedName(@event.OldValue, value.Value);
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public Task Consume(ConsumeContext<ValueOfMainProductDetailOptionValueHasBeenUpdated> context)
        => HandleAsync(context.Message);
}
