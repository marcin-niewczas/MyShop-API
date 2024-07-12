using MyShop.Application.Dtos.ECommerce.Categories;
using MyShop.Core.Abstractions;
using MyShop.Core.HelperModels;

namespace MyShop.Application.Dtos.ECommerce.Products;
public sealed record ProductFiltersDtoEc : IDto
{
    public required CategoryEcDto Category { get; init; }
    public required decimal? MinPrice { get; init; }
    public required decimal? MaxPrice { get; init; }
    public required IReadOnlyCollection<ProductOptionEc> ProductOptions { get; init; }
}
