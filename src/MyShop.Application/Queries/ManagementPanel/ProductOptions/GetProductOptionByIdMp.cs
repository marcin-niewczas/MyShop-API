using MyShop.Application.Dtos.ManagementPanel.ProductOptions;
using MyShop.Application.Responses;

namespace MyShop.Application.Queries.ManagementPanel.ProductOptions;
public sealed record GetProductOptionByIdMp(
    Guid Id
    ) : IQuery<ApiResponse<ProductOptionMpDto>>;
