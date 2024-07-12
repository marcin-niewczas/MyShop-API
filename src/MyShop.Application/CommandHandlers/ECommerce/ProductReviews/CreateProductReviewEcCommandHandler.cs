using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ECommerce.ProductReviews;
using MyShop.Application.Dtos.ECommerce.ProductReviews;
using MyShop.Application.Mappings;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.Products;
using MyShop.Core.Utils;

namespace MyShop.Application.CommandHandlers.ECommerce.ProductReviews;
internal sealed class CreateProductReviewEcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<CreateProductReviewEc, ApiResponse<ProductReviewMeEcDto>>
{
    public async Task<ApiResponse<ProductReviewMeEcDto>> HandleAsync(CreateProductReviewEc command, CancellationToken cancellationToken = default)
    {
        var userId = userClaimsService.GetUserClaimsData().UserId;

        var result = await unitOfWork.ProductRepository.AnyAsync(
             predicate: e => e.Id == command.ProductId,
             cancellationToken: cancellationToken
             );

        if (!result)
        {
            throw new NotFoundException(nameof(ProductReview), command.ProductId);
        }

        result = await unitOfWork.ProductReviewRepository.AnyAsync(
            predicate: e => e.ProductId == command.ProductId && e.RegisteredUserId == userId,
            cancellationToken: cancellationToken
            );

        if (result)
        {
            throw new BadRequestException($"Cannot add more than one {nameof(ProductReview).ToTitleCase()} per {nameof(Product)}.");
        }

        var productReview = new ProductReview(
            command.Review,
            command.Rate,
            userId,
            command.ProductId
            );

        productReview = await unitOfWork.ProductReviewRepository.AddAsync(productReview, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new(productReview.ToProductReviewMeEcDto());
    }
}
