using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.ValueObjects.ProductOptions;

namespace MyShop.Core.Models.Products;
public sealed class ProductProductDetailOptionValue : BaseTimestampEntity
{
    public Product Product { get; private set; } = default!;
    public Guid ProductId { get; private set; }
    public ProductDetailOptionValue ProductDetailOptionValue { get; private set; } = default!;
    public Guid ProductDetailOptionValueId { get; private set; }
    public ProductOptionPosition Position { get; private set; } = default!;

    private ProductProductDetailOptionValue() { }

    public ProductProductDetailOptionValue(
        Guid productId,
        Guid productDetailOptionValueId,
        ProductOptionPosition position
        )
    {
        ArgumentNullException.ThrowIfNull(nameof(position));

        ProductId = productId;
        ProductDetailOptionValueId = productDetailOptionValueId;
        Position = position;
    }

    public void UpdateProductDetailOptionValue(
        ProductDetailOptionValue productDetailOptionValue
        )
    {
        ArgumentNullException.ThrowIfNull(nameof(productDetailOptionValue));

        if (ProductDetailOptionValue is null)
        {
            throw new InvalidOperationException($"The {nameof(ProductDetailOptionValue)} must be included.");
        }

        if (ProductDetailOptionValue.Id == productDetailOptionValue.Id)
        {
            throw new BadRequestException($"Nothing change in {nameof(ProductProductDetailOptionValue)}.");
        }

        if (ProductDetailOptionValue.ProductOptionId != productDetailOptionValue.ProductOptionId)
        {
            throw new BadRequestException($"The Chosen{nameof(ProductDetailOptionValue)} not belong to right {nameof(ProductDetailOption)}.");
        }

        ProductDetailOptionValue = productDetailOptionValue;
        ProductDetailOptionValueId = productDetailOptionValue.Id;
    }

    public void UpdatePosition(ProductOptionPosition position)
    {
        ArgumentNullException.ThrowIfNull(nameof(position));

        if (ProductDetailOptionValue.ProductDetailOption.ProductOptionSubtype == ProductOptionSubtype.Main)
        {
            throw new BadRequestException($"Cannot change {ProductOptionSubtype.Main} {nameof(ProductProductDetailOptionValue)} {nameof(Position)}.");
        }

        if (Position <= 0)
        {
            throw new BadRequestException($"The {nameof(Position)} must be greater than {ProductOptionPosition.Min} for update.");
        }

        if (Position == position)
        {
            throw new BadRequestException($"The {nameof(Position)} is same for {nameof(ProductProductDetailOptionValue)} '{Id}'.");
        }

        Position = position;
    }
}
