using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ECommerce.ProductReviews;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.ValueObjects.Users;

namespace MyShop.Application.CommandHandlers.ECommerce.ProductReviews;
internal sealed class RemoveProductReviewEcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<RemoveProductReviewEc>
{
    public async Task HandleAsync(RemoveProductReviewEc command, CancellationToken cancellationToken = default)
    {
        var currentUser = userClaimsService.GetUserClaimsData();

        var productReview = await unitOfWork.ProductReviewRepository.GetByIdAsync(
            id: command.ProductReviewId,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductReview), command.ProductReviewId);

        if (currentUser.UserId != currentUser.UserId && !UserRole.HasEmployeePermission(currentUser.UserRole))
        {
            throw new ForbiddenException();
        }

        await unitOfWork.ProductReviewRepository.RemoveAsync(productReview);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
