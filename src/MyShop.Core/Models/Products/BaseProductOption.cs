using MyShop.Core.Models.BaseEntities;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Core.Models.Products;
public abstract class BaseProductOption : BaseTimestampEntity
{
    public ProductOptionName Name { get; protected set; } = default!;
    public ProductOptionSortType ProductOptionSortType { get; protected set; } = default!;
    public ProductOptionType ProductOptionType { get; private set; } = default!;
    public ProductOptionSubtype ProductOptionSubtype { get; protected set; } = default!;
    public abstract IReadOnlyCollection<BaseProductOptionValue> ProductOptionValues { get; }

    protected BaseProductOption() { }

    protected BaseProductOption(
        ProductOptionName name,
        ProductOptionType productOptionType,
        ProductOptionSubtype productOptionSubtype,
        ProductOptionSortType productOptionSortType
        )
    {
        Name = name;
        ProductOptionType = productOptionType;
        ProductOptionSubtype = productOptionSubtype;
        ProductOptionSortType = productOptionSortType;
    }
}
