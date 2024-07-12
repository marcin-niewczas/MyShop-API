using MyShop.Application.Dtos.ECommerce.ProductReviews;
using MyShop.Application.Dtos.ManagementPanel.ProductReviews;
using MyShop.Core.Models.Products;

namespace MyShop.Application.Mappings;
internal static class ProductReviewMappingExtension
{
    public static ProductReviewEcDto ToProductReviewEcDto(
        this ProductReview entity
        ) => new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Review = entity.Review,
            Rate = entity.Rate,
            UserFirstName = entity.RegisteredUser.FirstName,
            UserPhotoUrl = entity.RegisteredUser?.Photo?.Uri
        };

    public static ProductReviewMeEcDto ToProductReviewMeEcDto(
        this ProductReview entity
        ) => new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Review = entity.Review,
            Rate = entity.Rate,
        };

    public static ProductReviewMpDto ToProductReviewMpDto(
        this ProductReview entity
        ) => new()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Review = entity.Review,
            Rate = entity.Rate,
            UserFirstName = entity.RegisteredUser.FirstName,
            UserLastName = entity.RegisteredUser.LastName,
            UserPhotoUrl = entity.RegisteredUser?.Photo?.Uri
        };

    public static IReadOnlyCollection<ProductReviewEcDto> ToProductReviewEcDtos(
        this IEnumerable<ProductReview> entities
        ) => entities.Select(ToProductReviewEcDto).ToArray();

    public static IReadOnlyCollection<ProductReviewMpDto> ToProductReviewMpDtos(
        this IEnumerable<ProductReview> entities
        ) => entities.Select(ToProductReviewMpDto).ToArray();
}
