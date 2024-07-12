using MyShop.Core.Models.BaseEntities;
using MyShop.Core.ValueObjects.Photos;

namespace MyShop.Core.Models.Photos;
public abstract class Photo : BaseTimestampEntity
{
    public PhotoName Name { get; private set; } = default!;
    public PhotoExtension Extension { get; private set; } = default!;
    public PhotoContentType ContentType { get; private set; } = default!;
    public PhotoSize PhotoSize { get; private set; } = default!;
    public PhotoFilePath FilePath { get; private set; } = default!;
    public Uri Uri { get; private set; } = default!;
    public PhotoAlt Alt { get; private set; } = default!;
    public PhotoType PhotoType { get; private set; } = default!;

    protected Photo() { }

    protected Photo(
        PhotoName name,
        PhotoExtension photoExtension,
        PhotoContentType contentType,
        PhotoSize photoSize,
        PhotoFilePath filePath,
        Uri uri,
        PhotoAlt alt,
        PhotoType photoType
        )
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Extension = photoExtension ?? throw new ArgumentNullException(nameof(photoExtension));
        ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
        PhotoSize = photoSize ?? throw new ArgumentNullException(nameof(photoSize));
        FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        Uri = uri ?? throw new ArgumentNullException(nameof(uri));
        Alt = alt ?? throw new ArgumentNullException(nameof(alt));
        PhotoType = photoType ?? throw new ArgumentNullException(nameof(photoType));
    }
}
