using MyShop.Application.Dtos.ManagementPanel.ProductVariantPhotoItems;
using MyShop.Application.Responses;

namespace MyShop.Application.Queries.ManagementPanel.ProductVariantPhotoItems;
public sealed record GetProductVariantPhotoItemsMp(
    Guid Id
    ) : IQuery<ApiResponseWithCollection<ProductVariantPhotoItemMpDto>>;
