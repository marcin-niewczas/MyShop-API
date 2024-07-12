using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Core.Models.Products;
public sealed class ProductProductVariantOption : BaseTimestampEntity
{
    public Product Product { get; private set; } = default!;
    public Guid ProductId { get; private set; }
    public ProductVariantOption ProductVariantOption { get; private set; } = default!;
    public Guid ProductVariantOptionId { get; private set; }
    public ProductOptionPosition Position { get; private set; } = default!;

    public ProductProductVariantOption(
        Guid productId,
        Guid productVariantOptionId,
        ProductOptionPosition position
        )
    {
        ArgumentNullException.ThrowIfNull(nameof(position));

        ProductId = productId;
        ProductVariantOptionId = productVariantOptionId;
        Position = position;
    }

    private ProductProductVariantOption() { }

    public void UpdatePosition(ProductOptionPosition position)
    {
        ArgumentNullException.ThrowIfNull(nameof(position));

        if (ProductVariantOption.ProductOptionSubtype == ProductOptionSubtype.Main)
        {
            throw new BadRequestException($"Cannot change {ProductOptionSubtype.Main} {nameof(ProductProductVariantOption)} {nameof(Position)}.");
        }

        if (Position <= 0)
        {
            throw new BadRequestException($"The {nameof(Position)} must be greater than {ProductOptionPosition.Min} for update.");
        }

        if (Position == position)
        {
            throw new BadRequestException($"The {nameof(Position)} is same for {nameof(ProductProductVariantOption)} '{Id}'.");
        }

        Position = position;
    }
}
