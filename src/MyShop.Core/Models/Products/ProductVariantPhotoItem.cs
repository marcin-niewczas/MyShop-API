using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Photos;
using MyShop.Core.ValueObjects.Products;

namespace MyShop.Core.Models.Products;
public sealed class ProductVariantPhotoItem : BaseTimestampEntity
{
    public ProductVariant ProductVariant { get; private set; } = default!;
    public Guid ProductVariantId { get; private set; }
    public ProductVariantPhoto ProductVariantPhoto { get; private set; } = default!;
    public Guid ProductVariantPhotoId { get; private set; }
    public ProductVariantPhotoItemPosition Position { get; private set; } = default!;

    private ProductVariantPhotoItem() { }

    public ProductVariantPhotoItem(
        ProductVariant productVariant,
        Guid productVariantPhotoId,
        ProductVariantPhotoItemPosition position
        )
    {
        ProductVariant = productVariant ?? throw new ArgumentNullException(nameof(productVariant));
        ProductVariantId = productVariant.Id;
        ProductVariantPhotoId = productVariantPhotoId;
        Position = position ?? throw new ArgumentNullException(nameof(position));
    }

    public ProductVariantPhotoItem(
        ProductVariant productVariant,
        ProductVariantPhoto productVariantPhoto,
        ProductVariantPhotoItemPosition position
        )
    {
        ProductVariant = productVariant ?? throw new ArgumentNullException(nameof(productVariant));
        ProductVariantId = productVariant.Id;
        ProductVariantPhoto = productVariantPhoto ?? throw new ArgumentNullException(nameof(productVariantPhoto));
        ProductVariantPhotoId = productVariantPhoto.Id;
        Position = position ?? throw new ArgumentNullException(nameof(position));
    }

    public void UpdatePosition(ProductVariantPhotoItemPosition position)
    {
        ArgumentNullException.ThrowIfNull(nameof(position));

        if (Position == position)
        {
            throw new BadRequestException($"The {nameof(Position)} is same for {nameof(ProductVariantPhotoItem)} '{Id}'.");
        }

        Position = position;
    }
}
