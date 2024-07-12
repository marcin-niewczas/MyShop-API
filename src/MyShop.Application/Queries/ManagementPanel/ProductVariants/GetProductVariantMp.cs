using MyShop.Application.Responses;
using MyShop.Core.Dtos.ManagementPanel;

namespace MyShop.Application.Queries.ManagementPanel.ProductVariants;
public sealed record GetProductVariantMp(
    Guid Id
    ) : IQuery<ApiResponse<ProductVariantMpDto>>;
