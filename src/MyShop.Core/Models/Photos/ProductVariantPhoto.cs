using MyShop.Core.Models.Products;
using MyShop.Core.ValueObjects.Photos;

namespace MyShop.Core.Models.Photos;
public sealed class ProductVariantPhoto : Photo
{
    public IReadOnlyCollection<ProductVariant> ProductVariants { get; private set; } = default!;
    public IReadOnlyCollection<ProductVariantPhotoItem> ProductVariantPhotoItems { get; private set; } = default!;

    public ProductVariantPhoto(
        PhotoName name,
        PhotoExtension photoExtension,
        PhotoContentType contentType,
        PhotoSize photoSize,
        PhotoFilePath filePath,
        Uri uri,
        PhotoAlt alt
        ) : base(
                name,
                photoExtension,
                contentType,
                photoSize,
                filePath,
                uri,
                alt,
                PhotoType.ProductVariantPhoto
                )
    {
    }

    private ProductVariantPhoto() { }
}
