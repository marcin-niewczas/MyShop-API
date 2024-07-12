using MyShop.Application.Abstractions;
using MyShop.Application.Commands.ECommerce.ProductReviews;
using MyShop.Application.Dtos.ECommerce.ProductReviews;
using MyShop.Application.Mappings;
using MyShop.Application.Responses;
using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.Products;

namespace MyShop.Application.CommandHandlers.ECommerce.ProductReviews;
internal sealed class UpdateProductReviewEcCommandHandler(
    IUserClaimsService userClaimsService,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateProductReviewEc, ApiResponse<ProductReviewMeEcDto>>
{
    public async Task<ApiResponse<ProductReviewMeEcDto>> HandleAsync(UpdateProductReviewEc command, CancellationToken cancellationToken = default)
    {
        var registeredUserClaimsData = userClaimsService.GetUserClaimsData()
            as CustomerClaimsData ?? throw new ServerException($"The {nameof(UserClaimsData)} should be {nameof(CustomerClaimsData)} type.");

        var productReview = await unitOfWork.ProductReviewRepository.GetByIdAsync(
            id: command.Id,
            cancellationToken: cancellationToken
            ) ?? throw new NotFoundException(nameof(ProductReview), command.Id);

        productReview.Update(command.Review, command.Rate, registeredUserClaimsData);
        productReview = await unitOfWork.ProductReviewRepository.UpdateAsync(productReview);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new(productReview.ToProductReviewMeEcDto());
    }
}
