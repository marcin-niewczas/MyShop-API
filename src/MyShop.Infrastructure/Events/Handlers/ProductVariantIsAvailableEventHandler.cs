using MassTransit;
using Microsoft.EntityFrameworkCore;
using MyShop.Application.Abstractions;
using MyShop.Application.Events;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Notifications;
using MyShop.Core.Models.Products;
using MyShop.Core.ValueObjects.Notifications;
using MyShop.Core.ValueObjects.ProductOptions;
using MyShop.Infrastructure.Notifications.Senders.Interfaces;

namespace MyShop.Infrastructure.Events.Handlers;
internal sealed class ProductVariantIsAvailableEventHandler(
    IUnitOfWork unitOfWork,
    ICommonNotificationsSender commonNotificationsSender
    ) : IEventHandler<ProductVariantIsAvailable>,
        IConsumer<ProductVariantIsAvailable>
{
    public async Task HandleAsync(
        ProductVariantIsAvailable @event,
        CancellationToken cancellationToken = default
        )
    {
        var productVariant = await unitOfWork.ProductVariantRepository.GetByIdAsync(
            id: @event.Id,
            include: i => i.Include(e => e.Product)
                           .ThenInclude(e => e.ProductDetailOptionValues.Where(v => v.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main))
                           .ThenInclude(e => e.ProductDetailOption)
                           .Include(e => e.Product)
                           .ThenInclude(e => e.ProductProductVariantOptions.OrderBy(o => o.Position))
                           .Include(e => e.ProductVariantOptionValues)
                           .ThenInclude(e => e.ProductVariantOption),
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductVariant), @event.Id);

        var usersIdsWhoLikeThisProduct = await unitOfWork.FavoriteRepository.GetByPredicateAsync(
            predicate: e => e.EncodedProductVariantName == productVariant.EncodedName,
            selector: e => e.RegisteredUserId,
            cancellationToken: cancellationToken
            );

        if (usersIdsWhoLikeThisProduct.Count < 1)
        {
            return;
        }

        var notification = new Notification(
            NotificationType.ProductPriceReduced,
            $"The {productVariant.GetProductVariantFullName()} is available.",
            usersIdsWhoLikeThisProduct,
            productVariant.EncodedName
            );

        await unitOfWork.AddAsync(notification, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var tasks = usersIdsWhoLikeThisProduct.Select(id => commonNotificationsSender.SendAsync(
            id,
            notification,
            cancellationToken: cancellationToken
            ));

        await Task.WhenAll(tasks);
    }

    public Task Consume(ConsumeContext<ProductVariantIsAvailable> context)
        => HandleAsync(context.Message);
}
