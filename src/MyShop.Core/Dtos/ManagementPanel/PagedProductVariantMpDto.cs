using MyShop.Core.Abstractions;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.BaseEntities;

namespace MyShop.Core.Dtos.ManagementPanel;
public sealed record PagedProductVariantMpDto : BaseTimestampDto, IModel
{
    public required int Quantity { get; init; }
    public required decimal Price { get; init; }
    public required Guid SkuId { get; init; }
    public required string EncodedName { get; init; }
    public required IReadOnlyCollection<OptionNameValueId> ProductVariantValues { get; init; }
    public string VariantLabel
        => string.Join('/', ProductVariantValues.Select(v => v.Value));
}
