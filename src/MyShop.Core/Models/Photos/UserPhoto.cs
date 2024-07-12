using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.Photos;

namespace MyShop.Core.Models.Photos;
public sealed class UserPhoto : Photo
{
    public RegisteredUser RegisteredUser { get; private set; } = default!;

    public UserPhoto(
        PhotoName name,
        PhotoExtension photoExtension,
        PhotoContentType contentType,
        PhotoSize photoSize,
        PhotoFilePath filePath,
        Uri uri,
        PhotoAlt alt,
        RegisteredUser registeredUser
        )
        : base(
                name,
                photoExtension,
                contentType,
                photoSize,
                filePath,
                uri,
                alt,
                PhotoType.UserPhoto
                )
    {
        RegisteredUser = registeredUser ?? throw new ArgumentNullException(nameof(registeredUser));
    }

    private UserPhoto() { }
}
