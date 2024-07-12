using MyShop.Core.Exceptions;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.ProductReviews;
using MyShop.Core.ValueObjects.Users;

namespace MyShop.Core.Models.Products;
public sealed class ProductReview : BaseTimestampEntity
{
    public ProductReviewText Review { get; private set; }
    public ProductReviewRate Rate { get; private set; }
    public RegisteredUser RegisteredUser { get; private set; } = default!;
    public Guid RegisteredUserId { get; private set; }
    public Product Product { get; private set; } = default!;
    public Guid ProductId { get; private set; }

    public ProductReview(ProductReviewText review, ProductReviewRate rate, Guid registeredUserId, Guid productId)
    {
        if (registeredUserId == Guid.Empty)
        {
            throw new ArgumentException($"Parameter {nameof(registeredUserId)} cannot be default.", nameof(registeredUserId));
        }

        if (productId == Guid.Empty)
        {
            throw new ArgumentException($"Parameter {nameof(productId)} cannot be default.", nameof(productId));
        }

        Review = review;
        Rate = rate;
        RegisteredUserId = registeredUserId;
        ProductId = productId;
    }

    public void Update(
        ProductReviewText review,
        ProductReviewRate rate,
        CustomerClaimsData claimsData
        )
    {
        ArgumentNullException.ThrowIfNull(nameof(claimsData), nameof(claimsData));
        ArgumentNullException.ThrowIfNull(nameof(rate), nameof(rate));

        if (claimsData.UserId != RegisteredUserId && !UserRole.HasEmployeePermission(claimsData.UserRole))
        {
            throw new ForbiddenException();
        }

        if (review == Review && rate == Rate)
        {
            throw new BadRequestException("Nothing change.");
        }

        Rate = rate;
        Review = review;
    }
}
