using MyShop.Core.Exceptions;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Core.Models.Products;
public sealed class ProductDetailOptionValue : BaseProductOptionValue
{
    public ProductDetailOption ProductDetailOption { get; private set; } = default!;
    public IReadOnlyCollection<Product> Products { get; private set; } = default!;

    public ProductDetailOptionValue(
        ProductOptionValue value,
        Guid productDetailOptionId,
        ProductOptionPosition? position = null
        ) : base(value, position)
    {
        Discriminator = ProductOptionType.Detail;

        if (productDetailOptionId == default)
        {
            throw new ArgumentException(
                $"Parameter {nameof(productDetailOptionId)} cannot be default.", nameof(productDetailOptionId)
                );
        }

        ProductOptionId = productDetailOptionId;
    }

    private ProductDetailOptionValue() { }

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
}
