using MyShop.Application.Responses;
using MyShop.Core.Dtos.ManagementPanel;

namespace MyShop.Application.Queries.ManagementPanel.Products;
public sealed record GetProductMp(
    Guid Id
    ) : IQuery<ApiResponse<ProductMpDto>>;
