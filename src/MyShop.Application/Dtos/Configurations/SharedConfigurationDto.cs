using MyShop.Core.Abstractions;
using MyShop.Core.ValueObjects.ProductReviews;
using MyShop.Core.ValueObjects.ShoppingCarts;

namespace MyShop.Application.Dtos.Configurations;
public sealed record SharedConfigurationDto : IDto
{
    public int ProductReviewMaxRate { get; } = ProductReviewRate.Max;
    public string ServerTimeOffset { get; } = DateTimeOffset.Now.ToString("zzz");
    public int MaxShoppingCartItemQuantity { get; } = ShoppingCartItemQuantity.Max;
}
