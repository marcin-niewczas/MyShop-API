using MyShop.Application.Dtos.ManagementPanel.Photos;
using MyShop.Core.Abstractions;

namespace MyShop.Application.Dtos.ManagementPanel.ProductVariantPhotoItems;
public sealed record ProductVariantPhotoItemMpDto : BaseTimestampDto
{
    public required int Position { get; init; }
    public required PhotoMpDto Photo { get; init; }
}
