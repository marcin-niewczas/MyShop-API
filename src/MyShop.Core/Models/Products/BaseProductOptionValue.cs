using MyShop.Core.Models.BaseEntities;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Core.Models.Products;
public abstract class BaseProductOptionValue : BaseTimestampEntity
{
    public ProductOptionValue Value { get; protected set; } = default!;
    public ProductOptionPosition Position { get; protected set; } = default!;
    public ProductOptionType Discriminator { get; protected set; } = default!;
    public Guid ProductOptionId { get; protected set; } = default!;

    protected BaseProductOptionValue() { }

    protected BaseProductOptionValue(
        ProductOptionValue value,
        ProductOptionPosition? position = null
        )
    {
        Value = value;

        if (position is not null)
        {
            Position = position;
        }
    }
}
