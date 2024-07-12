using MyShop.Core.Exceptions;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Core.Models.Products;
public sealed class ProductVariantOptionValue : BaseProductOptionValue
{
    public ProductVariantOption ProductVariantOption { get; private set; } = default!;
    public IReadOnlyCollection<ProductVariant> ProductVariants { get; private set; } = default!;

    public ProductVariantOptionValue(
        ProductOptionValue value,
        Guid productVariantOptionId,
        ProductOptionPosition? position = null
        ) : base(value, position)
    {
        Discriminator = ProductOptionType.Variant;

        if (productVariantOptionId == default)
        {
            throw new ArgumentException(
                $"Parameter {nameof(productVariantOptionId)} cannot be default.", nameof(productVariantOptionId)
                );
        }

        ProductOptionId = productVariantOptionId;
    }

    public void Update(ProductOptionValue value)
    {
        if (value == Value)
        {
            throw new BadRequestException(
                $"The {nameof(ProductVariantOptionValue).ToTitleCase()} have the same {nameof(Value)} equals '{value}'."
                );
        }

        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public void UpdatePosition(ProductOptionPosition position)
    {
        Position = position ?? throw new ArgumentNullException(nameof(position));
    }

    private ProductVariantOptionValue() { }
}
