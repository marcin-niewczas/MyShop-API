using MyShop.Application.Commands.ManagementPanel.ProductReviews;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;

namespace MyShop.Application.CommandHandlers.ManagementPanel.ProductReviews;
internal sealed class RemoveProductReviewMpCommandHandler(
    IUnitOfWork unitOfWork
    ) : ICommandHandler<RemoveProductReviewMp>
{
    public async Task HandleAsync(RemoveProductReviewMp command, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.ProductReviewRepository.GetByIdAsync(
            id: command.Id,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductReview), command.Id);

        await unitOfWork.RemoveAsync(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
