using MyShop.Core.Models.MainPageSections;
using MyShop.Core.ValueObjects.Photos;

namespace MyShop.Core.Models.Photos;
public sealed class WebsiteHeroSectionPhoto : Photo
{
    public IReadOnlyCollection<WebsiteHeroSection> WebsiteHeroSections { get; private set; } = default!;
    public IReadOnlyCollection<WebsiteHeroSectionItem> WebsiteHeroSectionWebsiteHeroSectionPhotos { get; private set; } = default!;

    public WebsiteHeroSectionPhoto(
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
                PhotoType.WebsiteHeroPhoto
                )
    { }

    private WebsiteHeroSectionPhoto() { }
}
